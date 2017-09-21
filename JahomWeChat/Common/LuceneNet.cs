using JahomWeChat.Models.EntityModel;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace JahomWeChat.Common
{
	public class LuceneNet
	{
		public static ConcurrentQueue<Record> RecordsForCreateIndex = new ConcurrentQueue<Record>();

		public static void CreateIndex()
		{
			string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");//索引文档保存位置
			using (FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory()))
			{
				//是否存在索引库文件夹以及索引库特征文件
				//如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则返回
				var isExist = IndexReader.IndexExists(directory);
				if (isExist && IndexWriter.IsLocked(directory))
				{
					return;
				}
				//创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
				//补充:使用IndexWriter打开directory时会自动对索引库文件上锁
				using (IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED))
				{
					Record record = null;
					if (RecordsForCreateIndex.TryDequeue(out record))
					{
						//new一篇文档对象 一条记录对应索引库中的一个文档
						Document document = new Document();
						//向文档中添加字段  Add(字段,值,是否保存字段原始值,是否针对该列创建索引)
						//所有字段的值都将以字符串类型保存 因为索引库只存储字符串类型数据
						//Field.Store:表示是否保存字段原值。指定Field.Store.YES的字段在检索时才能用document.Get取出原值  
						//Field.Index.NOT_ANALYZED:指定不按照分词后的结果保存--是否按分词后结果保存取决于是否对该列内容进行模糊查询
						//Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
						//WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离
						document.Add(new Field("id", record.ID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
						document.Add(new Field("title", record.Title, Field.Store.YES, Field.Index.ANALYZED));
						document.Add(new Field("summary", record.Summary, Field.Store.YES, Field.Index.NOT_ANALYZED));
						document.Add(new Field("content", record.Content, Field.Store.NO, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

						writer.AddDocument(document); //文档写入索引库
					}
				}
			}
		}

		public static List<Record> SearchFromIndex(string searchKey)
		{
			string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");
			FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
			IndexReader reader = IndexReader.Open(directory, true);
			IndexSearcher searcher = new IndexSearcher(reader);
			//搜索条件
			BooleanQuery bQuery = new BooleanQuery();
			PhraseQuery tQuery = new PhraseQuery();
			PhraseQuery cQuery = new PhraseQuery();
			//把用户输入的关键字进行分词
			foreach (string word in SplitWords(searchKey))
			{
				tQuery.Add(new Term("title", word));
				cQuery.Add(new Term("content", word));
			}
			cQuery.Slop = 100; //指定关键词相隔最大距离
			bQuery.Add(tQuery, Occur.SHOULD);
			bQuery.Add(cQuery, Occur.SHOULD);

			//TopScoreDocCollector盛放查询结果的容器
			TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);
			searcher.Search(bQuery, null, collector);//根据query查询条件进行查询，查询结果放入collector容器
													 //TopDocs 指定0到GetTotalHits() 即所有查询结果中的文档 如果TopDocs(20,10)则意味着获取第20-30之间文档内容 达到分页的效果
			ScoreDoc[] docs = collector.TopDocs(0, collector.TotalHits).ScoreDocs;

			//展示数据实体对象集合
			List<Record> records = new List<Record>();
			for (int i = 0; i < docs.Length; i++)
			{
				int docId = docs[i].Doc;//得到查询结果文档的id（Lucene内部分配的id）
				Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document
				var record = new Record();
				record.Title = doc.Get("title");
				record.Summary = doc.Get("summary");
				record.ID = Guid.Parse(doc.Get("id"));
				records.Add(record);
			}

			return records;

		}

		public static string[] SplitWords(string keyWord)
		{
			List<string> strList = new List<string>();
			Analyzer analyzer = new PanGuAnalyzer();//指定使用盘古 PanGuAnalyzer 分词算法
			TokenStream tokenStream = analyzer.TokenStream("", new StringReader(keyWord));
			ITermAttribute token = null;
			while (tokenStream.IncrementToken())
			{
				token = tokenStream.GetAttribute<ITermAttribute>();
				strList.Add(token.Term); //得到分词后结果
			}
			return strList.ToArray();
		}
	}
}
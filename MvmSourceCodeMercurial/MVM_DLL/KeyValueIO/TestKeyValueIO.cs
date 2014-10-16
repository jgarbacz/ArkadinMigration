using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MVM
{
    public class TestStuff
    {
        public static void Main()
        {


            if (true)
            {
                Console.WriteLine("Test MergeReaderWriter with Number mix forward/reverse");

                MergeSortReaderWriter<Number, Number> rw
                    = new NaturalMergeSortReaderWriter<Number, Number>(2, 2, Path.GetTempPath(), MergeableComparer<Number>.Default, null, null);
                // add some rows to prove files get cut and merged.
                for (int i = 15; i >= 1; i--)
                {
                    rw.Add(new KeyValuePair<Number, Number>(new Number(i), new Number(i)), SortOrder.Reverse);
                }

                foreach (var kv in rw)
                {
                    Console.WriteLine(kv.Key.ToString() + "=" + kv.Value.ToString());
                }
            }
            //return;



            if (true)
            {
                var textCmp = MergeableComparer<Text>.Default;
                var textSer = Serializabler<Text>.Default;
               
                Console.WriteLine("Test MergeableFileRev");
                List<KeyValuePair<Text, Text>> list = new List<KeyValuePair<Text, Text>>();
                list.Add(new KeyValuePair<Text, Text>(new Text("a"), new Text("av")));
                list.Add(new KeyValuePair<Text, Text>(new Text("b"), new Text("bv")));
                list.Add(new KeyValuePair<Text, Text>(new Text("c"), new Text("cv")));
                string f = Path.GetTempFileName();

                MergeableFile<Text, Text> q = new MergeableFile<Text, Text>(Direction.Reverse, f, list, textSer, textSer);
                foreach (var kv in q)
                {
                    Console.WriteLine(kv.Key.Value + "=" + kv.Value.Value);
                }
            }
            //return;


            if (true)
            {
                Console.WriteLine("Test MergeReaderWriter Dynamic Keys, Text/Decimal mix, static value");

                // build up the dynamic key comparer
                IDynamicMergeableComparer dynamicTextComparer = DynamicMergeableComparer<Text>.Default;
                IDynamicMergeableComparer dynamicNumberComparer = DynamicMergeableComparer<Number>.DefaultDesc;
                IMergeableComparer<DynamicKey> dynamicKeyComparer
                    = new DynamicKeyComparer(
                        dynamicTextComparer,
                        dynamicNumberComparer);

                // build up the dynamic key serialization factory
                IDynamicSerializer dynamicTextSerializer = DynamicSerializer<Text>.Default;
                IDynamicSerializer dynamicNumberSerializer = DynamicSerializer<Number>.Default;
                ISerializer<DynamicKey> dynamicSerializer
                    = new DynamicKeySerializer(
                        dynamicTextSerializer,
                        dynamicNumberSerializer);

                // instanciate a merge sort reader/writer that takes a dynamic key and a static value.
                MergeSortReaderWriter<DynamicKey, Text> rw
                    = new NaturalMergeSortReaderWriter<DynamicKey, Text>(
                        2,
                        2,
                        Path.GetTempPath(),
                        dynamicKeyComparer,
                        dynamicSerializer,
                        null
                        );

                // Add some key/value pairs
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("a"), new Number(1) }), new Text("a1v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("a"), new Number(1.2m) }), new Text("a1v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("a"), new Number(2) }), new Text("a2v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("b"), new Number(2) }), new Text("b2v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("b"), new Number(1) }), new Text("b1v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("c"), new Number(1) }), new Text("c1v")), SortOrder.None);

                // loop thru results
                foreach (var kv in rw)
                {
                    Console.WriteLine(kv.Key.ToString() + "=" + kv.Value.ToString());
                }
            }
            //return;

            if (true)
            {
                Console.WriteLine("Test MergeReaderWriter Dynamic Keys, static value");

                // build up the dynamic key comparer
                IDynamicMergeableComparer dynamicNumberComparer = DynamicMergeableComparer<Number>.DefaultDesc;
                IMergeableComparer<DynamicKey> dynamicKeyComparer
                    = new DynamicKeyComparer(
                        dynamicNumberComparer);

                // build up the dynamic key serialization factory
                IDynamicSerializer dynamicNumberSerializer = DynamicSerializer<Number>.Default;
                ISerializer<DynamicKey> dynamicSerializer
                    = new DynamicKeySerializer(
                        dynamicNumberSerializer);

                // instanciate a merge sort reader/writer that takes a dynamic key and a static value.
                MergeSortReaderWriter<DynamicKey, Text> rw
                    = new NaturalMergeSortReaderWriter<DynamicKey, Text>(
                        2,
                        2,
                        Path.GetTempPath(),
                        dynamicKeyComparer,
                        dynamicSerializer,
                        null
                        );

                // Add some key/value pairs
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Number(1) }), new Text("1v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Number(2) }), new Text("2v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Number(3) }), new Text("3v")), SortOrder.None);
                for (int i = 4; i <= 15; i++)
                {
                    rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Number(i) }), new Text(i + "v")), SortOrder.None);
                }

                // loop thru results
                foreach (var kv in rw)
                {
                    Console.WriteLine(kv.Key.ToString() + "=" + kv.Value.ToString());
                }
            }
           // return;

            if (true)
            {
                Console.WriteLine("Test MergeReaderWriter with Number");

                MergeSortReaderWriter<Number, Number> rw
                    = new NaturalMergeSortReaderWriter<Number, Number>(2, 2, Path.GetTempPath(),MergeableComparer<Number>.Default,null,null);
                // add some rows to prove files get cut and merged.
                for (int i = 1; i <= 15; i++)
                {
                    rw.Add(new KeyValuePair<Number, Number>(new Number(i), new Number(i)), SortOrder.None);
                }

                foreach (var kv in rw)
                {
                    Console.WriteLine(kv.Key.ToString() + "=" + kv.Value.ToString());
                }
            }
            //return;

            if (true)
            {
                Console.WriteLine("Test MergeReaderWriter Dynamic Keys, static value");

                // build up the dynamic key comparer
                IDynamicMergeableComparer dynamicTextComparer = DynamicMergeableComparer<Text>.Default;
                IDynamicMergeableComparer dynamicTextComparerDesc = DynamicMergeableComparer<Text>.DefaultDesc;
                IMergeableComparer<DynamicKey> dynamicKeyComparer 
                    = new DynamicKeyComparer(
                        dynamicTextComparer,
                        dynamicTextComparerDesc);

                // build up the dynamic key serialization factory
                IDynamicSerializer dynamicTextSerializer = DynamicSerializer<Text>.Default;
                ISerializer<DynamicKey> dynamicSerializer 
                    = new DynamicKeySerializer(
                        dynamicTextSerializer,
                        dynamicTextSerializer);

                // instanciate a merge sort reader/writer that takes a dynamic key and a static value.
                MergeSortReaderWriter<DynamicKey, Text> rw
                    = new NaturalMergeSortReaderWriter<DynamicKey, Text>(
                        2,
                        2,
                        Path.GetTempPath(), 
                        dynamicKeyComparer,
                        dynamicSerializer, 
                        null
                        );
                
                // Add some key/value pairs
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("a"), new Text("1") }), new Text("a1v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("a"), new Text("2") }), new Text("a2v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("b"), new Text("2") }), new Text("b2v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("b"), new Text("1") }), new Text("b1v")), SortOrder.None);
                rw.Add(new KeyValuePair<DynamicKey, Text>(new DynamicKey(new object[] { new Text("c"), new Text("1") }), new Text("c1v")), SortOrder.None);
                
                // loop thru results
                foreach (var kv in rw)
                {
                    Console.WriteLine(kv.Key.ToString() + "=" + kv.Value.ToString());
                }
            }
            //return;
            
            if (true)
            {
                Console.WriteLine("Test MergeReaderWriter");
                //MergeSortReaderWriter<Text, Text> rw = new NaturalMergeSortReaderWriter<Text, Text>(null, 2, 2, Path.GetTempPath());

                // need way to bundle live and raw comparer
                // then way to flip them.
                //var cmp = MergeableComparer<Text>.DefaultDesc;
                var cmp = MergeableComparer<Text>.DefaultDesc;
               
                MergeSortReaderWriter<Text, Text> rw
                    = new NaturalMergeSortReaderWriter<Text, Text>(2, 2, Path.GetTempPath(), cmp, null, null);

                // add some rows to prove it will reorder
                //rw.Add(new KeyValuePair<Text, Text>(new Text("a"), new Text("av")), false);
                //rw.Add(new KeyValuePair<Text, Text>(new Text("c"), new Text("cv")), false);
                //rw.Add(new KeyValuePair<Text, Text>(new Text("b"), new Text("bv")), false);
                //rw.Add(new KeyValuePair<Text, Text>(new Text("d"), new Text("dv")), false);

                // add some rows to prove files get cut and merged.
                for (int i = 1; i <= 15; i++)
                {
                    string key = "k" + i.ToString().PadLeftFixed(3,'0');
                    string val = key + "v";
                    rw.Add(new KeyValuePair<Text, Text>(new Text(key), new Text(val)), SortOrder.None);
                }

                foreach (var kv in rw)
                {
                    Console.WriteLine(kv.Key.Value + "=" + kv.Value.Value);
                }
            }
            //return;
            if (true)
            {
                Console.WriteLine("Test MergeableSortingQueue DESC!");
                var liveCmp = new DescLiveComparer<Text>(new DefaultLiveComparer<Text>());
                MergeableSortingQueue<Text, Text> sortingQueue = new MergeableSortingQueue<Text, Text>(liveCmp);
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("a"), new Text("av")));
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("c"), new Text("cv")));
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("b"), new Text("bv")));
                foreach (var kv in sortingQueue)
                {
                    Console.WriteLine(kv.Key.Value + "=" + kv.Value.Value);
                }
            }
           // return;
            if (true)
            {
                Console.WriteLine("Test LiveToRawEnumerator");
                MergeableSortingQueue<Text, Text> sortingQueue = new NaturalMergeableSortingQueue<Text, Text>();
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("a"), new Text("av")));
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("c"), new Text("cv")));
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("b"), new Text("bv")));
                Console.WriteLine("Created MergeableSortingQueue with count=" + sortingQueue.Count);

                LiveToRawEnumerator<Text, Text> rawEnum = new LiveToRawEnumerator<Text, Text>(sortingQueue.GetMergeEnumerator(),null,null);

                Console.WriteLine("Created LiveToRawEnum with count=" + rawEnum.Count);

                //while (rawEnum.MoveNext())
                //{
                //    Console.WriteLine("RAWKEY:" + rawEnum.Current.Key.AsciiValue);
                //}

                RawMergeSortReader<Text, Text> rawMergeReader = new NaturalRawMergeSortReader<Text, Text>();
                // advance raw to next value to be merged..
                rawMergeReader.InsertMergeEnumerator(rawEnum);
                Console.WriteLine("Created mergeEnum with count=");
                foreach (var kv in rawMergeReader)
                {
                    Console.WriteLine("RAWKEY:" + kv.Key.AsciiValue);
                }
            }

           // return;

            if (true)
            {
                Console.WriteLine("Test MergeSortReader");
                MergeSortReader<Text, Text> reader = new NaturalMergeSortReader<Text, Text>();
                {
                    MergeableSortingQueue<Text, Text> q = new NaturalMergeableSortingQueue<Text, Text>();
                    q.Add(new KeyValuePair<Text, Text>(new Text("a1"), new Text("a1v")));
                    q.Add(new KeyValuePair<Text, Text>(new Text("c2"), new Text("c2v")));
                    q.Add(new KeyValuePair<Text, Text>(new Text("b1"), new Text("b1v")));
                    reader.InsertMergeEnumerable(q);
                }
                {
                    MergeableQueue<Text, Text> q = new MergeableQueue<Text, Text>();
                    q.Add(new KeyValuePair<Text, Text>(new Text("a2"), new Text("a2v")));
                    q.Add(new KeyValuePair<Text, Text>(new Text("b1"), new Text("b1v")));
                    q.Add(new KeyValuePair<Text, Text>(new Text("c1"), new Text("c1v")));
                    reader.InsertMergeEnumerable(q);
                }
                foreach (var kv in reader)
                {
                    Console.WriteLine(kv.Key.Value + "=" + kv.Value.Value);
                }
            }
           // return;

           

           

            if (true)
            {
                Console.WriteLine("Test MergeSortReader");
                MergeSortReader<Text, Text> reader = new NaturalMergeSortReader<Text, Text>();
                {
                    MergeableSortingQueue<Text, Text> q = new NaturalMergeableSortingQueue<Text, Text>();
                    q.Add(new KeyValuePair<Text, Text>(new Text("a1"), new Text("a1v")));
                    q.Add(new KeyValuePair<Text, Text>(new Text("c2"), new Text("c2v")));
                    q.Add(new KeyValuePair<Text, Text>(new Text("b1"), new Text("b1v")));
                    reader.InsertMergeEnumerable(q);
                }
                {
                    MergeableQueue<Text, Text> q = new MergeableQueue<Text, Text>();
                    q.Add(new KeyValuePair<Text, Text>(new Text("a2"), new Text("a2v")));
                    q.Add(new KeyValuePair<Text, Text>(new Text("b1"), new Text("b1v")));
                    q.Add(new KeyValuePair<Text, Text>(new Text("c1"), new Text("c1v")));
                    reader.InsertMergeEnumerable(q);
                }
                foreach (var kv in reader)
                {
                    Console.WriteLine(kv.Key.Value + "=" + kv.Value.Value);
                }
            }
           // return;

            

           
          

            //TestSerializeText();
            // how do i sort binary...
            // we got the concept of a K,V row where K knows its len and V knows its len.

            if (true)
            {
                Console.WriteLine("Test MergeableFile to then raw..");
                List<KeyValuePair<Text, Text>> list = new List<KeyValuePair<Text, Text>>();
                list.Add(new KeyValuePair<Text, Text>(new Text("a"), new Text("av")));
                list.Add(new KeyValuePair<Text, Text>(new Text("b"), new Text("bv")));
                list.Add(new KeyValuePair<Text, Text>(new Text("c"), new Text("cv")));
                string f = Path.GetTempFileName();
                MergeableFile<Text, Text> q = new MergeableFile<Text, Text>(Direction.Forward, f, list, null, null);
                var eq = q.GetMergeEnumerator();

                Console.WriteLine("read one value");
                if (eq.MoveNext()) Console.WriteLine("LIVE:"+eq.Current.Key.Value + "=" + eq.Current.Value.Value);

                Console.WriteLine("go raw for the rest");

                var req = q.GetRawKeyValuePairEnumerator();
                while(req.MoveNext())
                {
                    Console.WriteLine("RAW:" + req.Current.Key.AsciiValue+"="+req.Current.Value.AsciiValue);
                }

            }
           
            
            
            if (true)
            {
                Console.WriteLine("Test MergeableFile");
                List<KeyValuePair<Text, Text>> list = new List<KeyValuePair<Text, Text>>();
                list.Add(new KeyValuePair<Text, Text>(new Text("a"), new Text("av")));
                list.Add(new KeyValuePair<Text, Text>(new Text("b"), new Text("bv")));
                list.Add(new KeyValuePair<Text, Text>(new Text("c"), new Text("cv")));
                string f = Path.GetTempFileName();
                MergeableFile<Text, Text> q = new MergeableFile<Text, Text>(Direction.Reverse, f, list, null, null);
                foreach (var kv in q)
                {
                    Console.WriteLine(kv.Key.Value + "=" + kv.Value.Value);
                }
            }

            //return;
            if (true)
            {
                Console.WriteLine("Test MergeableSortingQueue");
                MergeableSortingQueue<Text, Text> sortingQueue = new NaturalMergeableSortingQueue<Text, Text>();
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("a"), new Text("av")));
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("c"), new Text("cv")));
                sortingQueue.Add(new KeyValuePair<Text, Text>(new Text("b"), new Text("bv")));
                foreach (var kv in sortingQueue)
                {
                    Console.WriteLine(kv.Key.Value + "=" + kv.Value.Value);
                }
            }
            
            if (true)
            {
                Console.WriteLine("Test MergeableQueue");
                MergeableQueue<Text, Text> q = new MergeableQueue<Text, Text>();
                q.Add(new KeyValuePair<Text, Text>(new Text("a"), new Text("av")));
                q.Add(new KeyValuePair<Text, Text>(new Text("c"), new Text("cv")));
                q.Add(new KeyValuePair<Text, Text>(new Text("b"), new Text("bv")));
                foreach (var kv in q)
                {
                    Console.WriteLine(kv.Key.Value + "=" + kv.Value.Value);
                }
            }
            
           


            // our base case is to merge an object with a file to a file... so that is
            // what we need to tune for... this means we are going to serialize anyways
            // so goto bytes then merge. DONE.

            // our other base case is that we need to loop thru the values!!
        }


        public static void TestSerializeNumber()
        {
            // test serialize deserialize...
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            BinaryReader br = new BinaryReader(ms);

            int count = 3;

            for (int i = 0; i < count; i++)
            {
                Number t = new Number(i);
                t.Serialize(bw);
            }

            ms.Position = 0;

            for (int i = 0; i < count; i++)
            {
                Number t = new Number();
                t.Deserialize(br);
                Console.WriteLine(t.Value);
            }
        }


        public static void TestSerializeText()
        {
            // test serialize deserialize...
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            BinaryReader br = new BinaryReader(ms);

            int count = 3;

            for (int i = 0; i < count; i++)
            {
                Text t = new Text("hi rob " + i);
                t.Serialize(bw);
            }

            ms.Position = 0;

            for (int i = 0; i < count; i++)
            {
                Text t = new Text();
                t.Deserialize(br);
                Console.WriteLine(t.Value);
            }
        }

    }

}

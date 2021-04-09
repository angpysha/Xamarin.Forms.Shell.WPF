//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProjectClipboard.WPF.NETFX4.Helpers
//{
//    internal class StringCollectionEnumerable : IEnumerable<string>
//    {
//        private StringCollection underlyingCollection;

//        public StringCollectionEnumerable(StringCollection underlyingCollection)
//        {
//            this.underlyingCollection = underlyingCollection;
//        }

//        public IEnumerator<string> GetEnumerator()
//        {
//            return new StringEnumeratorWrapper(underlyingCollection.GetEnumerator());
//        }

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            return this.GetEnumerator();
//        }
//    }

//    internal class StringEnumeratorWrapper : IEnumerator<string>
//    {
//        private StringEnumerator underlyingEnumerator;

//        public StringEnumeratorWrapper(StringEnumerator underlyingEnumerator)
//        {
//            this.underlyingEnumerator = underlyingEnumerator;
//        }

//        public string Current
//        {
//            get
//            {
//                return this.underlyingEnumerator.Current;
//            }
//        }

//        public void Dispose()
//        {
//            // No-op 
//        }

//        object System.Collections.IEnumerator.Current
//        {
//            get
//            {
//                return this.underlyingEnumerator.Current;
//            }
//        }

//        public bool MoveNext()
//        {
//            return this.underlyingEnumerator.MoveNext();
//        }

//        public void Reset()
//        {
//            this.underlyingEnumerator.Reset();
//        }
//    }
//}

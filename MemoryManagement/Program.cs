using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace MemoryManagement
{
    public class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World. I am disposing somethings!");

            //DisposeExample dispExample = new DisposeExample();
            //int num1 = 20;
            int num2 = 25;
            IntPtr myPtr = new IntPtr(5);
            IntPtr.Add(myPtr, num2);
            using (DisposeExample.MyResources ddd = new DisposeExample.MyResources(myPtr))
            {
                Console.WriteLine("I am using the pointer");
            }

            //WordCount myFile = new WordCount(@"C:\Users\DIDA\Desktop\RudzaniReader.txt");
            Console.WriteLine("I am done");
            Console.ReadLine();
        }

    }

    public class DisposeExample
    {
        // A base class that implements IDisposable.
        // By implementing IDisposable, you are announcing that
        // instances of this type allocate scarce resources.
        public class MyResources : IDisposable
        {
            private IntPtr handle;
            private Component component = new Component();

            // Track whether Dispose has been called.
            private bool disposed = false;

            public MyResources(IntPtr handle)
            {
                this.handle = handle;
                Console.WriteLine("I am {0}", handle);
            }

            // Implement IDisposable.
            // Do not make this method virtual.
            // A derived class should not be able to override this method.
            public void Dispose()
            {
                Console.WriteLine("I am using calling Dispose");
                Dispose(true);

                // This object will be cleaned up by the Dispose method.
                // Therefore, you should call GC.SupressFinalize to
                // take this object off the finalization queue
                // and prevent finalization code for this object
                // from executing a second time.
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        Console.WriteLine("I am using the Dispose");
                        // Dispose managed resources.
                        component.Dispose();
                    }

                    // Call the appropriate methods to clean up
                    // unmanaged resources here.
                    // If disposing is false,
                    // only the following code is executed.
                    CloseHandle(handle);
                    handle = IntPtr.Zero;

                    Console.WriteLine("I am handle = IntPtr.Zero");
                    Console.WriteLine("I am {0}", handle);
                    disposed = true;
                }
            }

            // Use interop to call the method necessary
            // to clean up the unmanaged resource.
            [System.Runtime.InteropServices.DllImport("Kernel32")]
            private extern static Boolean CloseHandle(IntPtr handle);

            // Use C# destructor syntax for finalization code.
            // This destructor will run only if the Dispose method
            // does not get called.
            // It gives your base class the opportunity to finalize.
            // Do not provide destructors in types derived from this class.
            ~MyResources()
            {
                Console.WriteLine("I am using the finalize");
                Dispose(false);
            }

        }
    }

    public class WordCount
    {
        private String filename = String.Empty;
        private int nWords = 0;
        private String pattern = @"\b\w+\b";

        public WordCount(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("The file does not exist.");

            this.filename = filename;
            string txt = String.Empty;
            using (StreamReader sr = new StreamReader(filename))
            {
                txt = sr.ReadToEnd();
            }
            nWords = Regex.Matches(txt, pattern).Count;
        }

        public string FullName
        { get { return filename; } }

        public string Name
        { get { return Path.GetFileName(filename); } }

        public int Count
        { get { return nWords; } }
    }
}

namespace FilesProcessingExample.Include
{
    public class BadCastsExample
    {
        public void Do()
        {
            string source = "IDDQD";
            string dest = (string) source;
        }
    }
}

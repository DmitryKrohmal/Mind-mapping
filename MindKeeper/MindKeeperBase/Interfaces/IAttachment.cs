namespace MindKeeperBase.Interfaces
{
    public interface IAttachment
    {
        string FileName { get;}
        byte[] FileData { get; set; }
        void Initialize(string path);
        string PathToFile { get; set; }
    }
}
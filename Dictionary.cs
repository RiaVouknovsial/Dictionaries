abstract class Dictionary
{
    public abstract void SaveDictionaryToFile(string filePath);
    public abstract void LoadDictionaryFromFile(string filePath);
    public abstract void AddWord();
    public abstract void ReplaceWord();
    public abstract void RemoveWord();
    public abstract void SearchTranslation();
    public abstract void ViewFileContent(string filename);
    public abstract void ExportDictionaryToTxt(string filePath);
}
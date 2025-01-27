﻿namespace AiPainter;

class StoredImageList
{
    private readonly List<StoredImageItem> images = new();

    public int Count => images.Count;

    public bool Update()
    {
        var allFiles = Directory.GetFiles(Program.Config.InvokeAiOutputFolderPath, "*.png");

        var removeCount = images.RemoveAll(x => !allFiles.Contains(x.FilePath));

        var wasUpdate = false;
        foreach (var image in images)
        {
            if (image.Update()) wasUpdate = true;
        }
        
        var newFiles = allFiles.Where(x => images.All(y => y.FilePath != x)).ToArray();
        images.AddRange(newFiles.Select(x => new StoredImageItem(x)));
            
        if (removeCount > 0 || wasUpdate || newFiles.Length > 0)
        {
            images.Sort((a, b) => DateTime.Compare(b.ChangeTime, a.ChangeTime));
            return true;
        }

        return false;
    }

    public void Remove(string filePath)
    {
        images.RemoveAll(y => y.FilePath == filePath);
    }

    public StoredImageItem GetAt(int index) => images[index];
}
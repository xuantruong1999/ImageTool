using ImageResizeTool;


string sourcePath = "C:\\Users\\Truong\\Documents\\testtool\\new";
string nameFolder = "resized";
ImageResize.ResizeImage(Path.Combine(sourcePath),
 Path.Combine(sourcePath, nameFolder), 800, 600);

 //ImageResize.ConvertImageFormat(sourcePath,Path.Combine(sourcePath, nameFolder),"png");


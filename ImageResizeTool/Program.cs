using ImageResizeTool;


string sourcePath = "C:\\Users\\Truong\\Documents\\testtool\\new\\resized";
string nameFolder = "compressed";
// ImageResize.ResizeImage(Path.Combine(sourcePath),
//  Path.Combine(sourcePath, nameFolder), 800, 600);
 //ImageResize.ConvertImageFormat(sourcePath,Path.Combine(sourcePath, nameFolder),"png");


ImageResize.CompressImage(sourcePath, Path.Combine(sourcePath, nameFolder), 90, true);

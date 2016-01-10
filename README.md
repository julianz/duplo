# Duplo

A duplicate file finder, particularly intended to work with collections of digital photos where
you may have copied a card or folder full of pics multiple times.

Give Duplo one or more directories full of images and it will examine them to find duplicates. 
Currently it narrows down the search by file name and size, then confirms it's suspicions by
hashing the first chunk of each file.

 - It can throw a false positive if the first section of a file is identical to another, for
   example BMP screenshots where the top bit of the screen looks the same.

 - Duplo will only find files that are an exact match, it's not doing any kind of fuzzy image 
   matching.
 
Duplo writes it's findings out to a couple of JSON files in the current directory, and highlights 
folders with large numbers/percentages of dupes on the command line.

## Usage

`duplo.exe <target-dir> [<target-dir> ...]`

## Plans

 - add loading of previous result files so you don't need to go through all the files repeatedly.
 
 - improve the presentation of results, maybe with an HTML viewer.
 
 - actually use it to cut down the ridiculous number of duplicate photos in my archive.

 - add some config about what and where to output.
 
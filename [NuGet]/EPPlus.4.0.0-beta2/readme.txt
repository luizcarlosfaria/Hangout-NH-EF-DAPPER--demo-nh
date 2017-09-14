EPPlus 4.0.0 Beta 2

Visit epplus.codeplex.com for the latest information

EPPlus-Create Advanced Excel spreadsheet.

This is a beta release. Please test and give us some feedback

New features

Replaced Packaging API with DotNetZip
* This will remove any problems with Isolated Storage and enable multi threading


New Cell store
* Less memory consumtion
* Insert columns (not on the range level)
* Faster row inserts,

Formula Parser
* Calculates all formulas in a workbook, a worksheet or in a specified range
* 100+ functions implemented
* Access via Calculate methods on Workbook, Worksheet and Range objects.
* Add custom/missing Excel functions via Workbook.FormulaParserManager.
* Samples added to the EPPlusSamples project.

The formula parser does not support Array Formulas
* Intersect operator (Space)
* References to external workbooks
* And probably a whole lot of other stuff as well :)

Perfomance
*Of course the performance of the formula parser is nowhere near Excels.Our focus has been functionality.

Agile Encryption (Office 2012-)
* Support for newer type of encryption.

Minor new features
* Chart worksheets
* New Chart Types Bubblecharts
* Radar Charts
* Area Charts
* And lots of bugfixes...

Beta 2 Changes
* Fixed bug when using RepeatColumns & RepeatRows at the same time.
* VBA project will be left untouched if its not accessed.
* Fixed problem with strings on save.
* Added locks to the cellstore for access by mulitple threads.
* Implemented Indirect function
* Used DisplayNameAttribute to generate column headers from LoadFromCollection
* Rewrote ExcelRangeBase.Copy function. 
* Added caching to Save ZipStream for Cells and shared strings to speed up the Save method.
* Added Missing InsertColumn and DeleteColumn
* Added pull request to support Date1904 
* Added pull request ExcelWorksheet.LoadFromDataReader


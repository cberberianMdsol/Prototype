Feature: TextFileConversionFeature
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Create Text File
	Given A file has been dropped for conversion
	When I press call DataFileConversion Api
	Then the result is a new file location returned

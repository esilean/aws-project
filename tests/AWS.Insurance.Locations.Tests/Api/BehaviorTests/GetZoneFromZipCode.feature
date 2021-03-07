Feature: GetZoneFromZipCode
	In order to use this service to calculate a tax
	From an ZipCode
	I want to get the Zone Area

Scenario: Discover Zone Area from a Customer
	When I call the LocationApi passing a ZipCode: '<zipCode>'
	Then the result should be 200
	Examples: 
	| zipCode |
	| 1000    |
	| 1500    |
	| 3999    |
	| 5500    |
	| 7800    |
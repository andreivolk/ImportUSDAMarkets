# ImportUSDAMarkets
Import Farmers Markets details provided on the USDA website as a CSV into MongoDB database.

Uses CsvHelper by Josh Close https://github.com/joshclose/csvhelper

USDA data can be found at https://www.ams.usda.gov/local-food-directories/farmersmarkets

Add appropriate keys/values to AppSettingsSecrets.config and run the program.

Note: for the CsvHelper to map to your class the field names must match the headers in the CSV file <b>exactly</b>.

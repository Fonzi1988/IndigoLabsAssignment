# IndigoLabs assignment

This web API exposes corona cases in Slovene regions.

The API exposes two endpoints.

 1. endpoint *"/api/regions/cases"*  returns a list of daily corona cases (active, vaccinated 1st, vaccinted 2nd, deceased) for Slovene regions. It accepts three query parrameters:
	 - region - possible values (LJ, CE, KR, NM, KK, KP, MB, MS, NG, PO, SG, 
   ZA)
	 - from - from date   
	 -  to - to date
	 - example  *"api/region/cases?from=2021-10-17&to=2021-10-17&region=nm"*
2. endpoint *"/api/regions//lastweek"* returns a list of total active corona cases in the last week for all Slovene regions

For testing purposes the credentials are hardcoded to "User:password"

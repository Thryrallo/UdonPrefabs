import os
import re
import holidays as hl

output = ""

years_start = 2010
years_end = 2030

month_names = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]

# create dictionary key date, value list of holidays
holidays = {}

def add_holidays_for_code(code, year):
    countryHolidays = hl.CountryHoliday(code, years=year).items()
    for date, name in countryHolidays:
        date = date.strftime("%Y-%m-%d")
        if date not in holidays:
            holidays[date] = []
        if name not in holidays[date]:
            holidays[date].append(name)
            

for year in range(years_start, years_end + 1):
    # get us holidays of us, germany, uk, france
    add_holidays_for_code("US", year)
    add_holidays_for_code("DE", year)
    add_holidays_for_code("GB", year)
    add_holidays_for_code("FR", year)

# add internation days from unesco https://aspnet.unesco.org/en-us/international-days
# is in format
# 8 March
# International Women’s Day
# with possible empty lines in between
with open(os.path.join(os.path.dirname(__file__), "unesco.txt"), "r", encoding="utf-8") as f:
    # read all lines
    lines = f.readlines()
    # remove empty lines
    lines = [line for line in lines if line.strip() != ""]
    for i in range(0, len(lines), 2):
        date = lines[i].strip()
        name = lines[i + 1].strip()
        month = date.split(" ")[1]
        month = month_names.index(month)
        day = int(date.split(" ")[0])
        for year in range(years_start, years_end + 1):
            # format to yyyy-mm-dd
            date = f"{year}-{month:02d}-{day:02d}"
            # add to holidays
            if date not in holidays:
                holidays[date] = []
            if name not in holidays[date]:
                holidays[date].append(name)

# turn into csv string
for date, names in holidays.items():
    if len(names) == 0:
        continue
    output += f"{date};"
    for name in names:
        output += f"{name}&"
    # remove last &
    output = output[:-1]
    output += "\n"

# remove last newline
output = output[:-1]

# write to file
openFile = open(os.path.join(os.path.dirname(__file__), "holidays.txt"), "w", encoding="utf-8")
openFile.write(output)
openFile.close()
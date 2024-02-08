import os
import re
import holidays as hl
import csv

output = ""

years_start = 2010
years_end = 2030

month_names = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]

# create dictionary key date, value list of holidays
holidays = {}

def add_entry(datestr, name, color):
    if datestr not in holidays:
        holidays[datestr] = {}
    if name not in holidays[datestr]:
        holidays[datestr][name] = color

def add_holidays_for_code(code, year):
    countryHolidays = hl.CountryHoliday(code, years=year).items()
    for date, name in countryHolidays:
        date = date.strftime("%Y-%m-%d")
        add_entry(date, name, "(1,1,1)")
            

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
        add_entry(f"{month:02d}-{day:02d}", name, "(1,1,1)")

# add birthdays from birthdays.csv
# Birthdays are loaded by themself now
# with open(os.path.join(os.path.dirname(__file__), "birthdays.csv"), "r", encoding="utf-8") as f:
#     reader = csv.reader(f, delimiter=";")
#     for row in reader:
#         name = row[0]
#         day = int(row[1])
#         month = int(row[2])
#         hexColor = row[3]
#         # convert hex to rgb
#         r = int(hexColor[1:3], 16) / 255
#         g = int(hexColor[3:5], 16) / 255
#         b = int(hexColor[5:7], 16) / 255
#         for year in range(years_start, years_end + 1):
#             date = f"{year}-{month:02d}-{day:02d}"
#             add_entry(date, name+"'s Birthday", f"({r},{g},{b})")

# add hardcoded yearly holidays
custom_holidays = [(10,9,"Thry's Birthday", "(1,0.5,1)")]
for month, day, name, color in custom_holidays:
    date = f"{month:02d}-{day:02d}"
    add_entry(date, name, color)

# turn into csv string
for date, entries in holidays.items():
    if(len(entries) == 0):
        continue
    output += f"{date};"
    for name, color in entries.items():
        # clear name
        name = name.replace(";", "").replace("\n", "")
        output += f"{name};{color};"
    # remove last &
    output = output[:-1]
    output += "\n"

# remove last newline
output = output[:-1]

# write to file
openFile = open(os.path.join(os.path.dirname(__file__), "holidays.txt"), "w", encoding="utf-8")
openFile.write(output)
openFile.close()
#  This file is designed to take LEO satellite data from a nasa site in 3LE file format and parse it using an
#  Aplha-5 schema to generate satellites for the Space Simulator LA Hacks 2021 project
#  Code written by Allison LeBus

from itertools import islice
import pandas as pd
import numpy as np


# fixing the decimal place in given eccentricity data
def fix_eccent(num):
    num = num * (10 ** -7)
    return '{:.7f}'.format(num)


# --------------------------------------Data Frame Structure------------------------------------------------------------
# Name of the variable[line][start char: end char: step(1)] - strip if \n exist in variable
# Name[0][2:24:1]           --strip
# 1st Der M[1][33:43:1]
# # 2nd Derv M[1][44:521]   --strip, float, for future use
# Incl[2][8:16:1]
# RoA[2][17:25:1]
# Eccent[2][26:33:1]        --multiply by 10E-7
# Arg of Peri[2][34:42:1]
# Mean Anom[2][43:51:1]
# Mean Motion[2][52:63:1]
# # Drag(B)[1][53:61:1]     --needs extra parsing, for future use
col_names = ['Name', '1st Der M', 'Incl', 'RoA', 'Eccent',
             'Arg of Peri', 'Mean Anom', 'Mean Motion']

data = {'Name': [],
        '1st Der M': [],
        'Incl': [],
        'RoA': [],
        'Eccent': [],
        'Arg of Peri': [],
        'Mean Anom': [],
        'Mean Motion': []}

df = pd.DataFrame(data, columns=col_names)

# ------------------------------Reading from the 3LE file in text format------------------------------------------------
# the number of satellites and kind can be manipulated by editing sat_tle_dat.txt in notepad++ by appending
# and deleting different 3le files, choices: LEO, MEO, HEO, and Geosynchronous
# this with block handles opening and closing the file
with open('sat_tle_dat.txt') as reader:
    while True:
        s = list(islice(reader, 3))
        if not s:  # breaks if the file cannot be sliced ie. has no more lines
            break

        # ----- this code is very slow, write to these new formatted lines to a file that is easy to read in C# --------
        # take values and return them as floats as a function
        values = np.array([[s[0][2:24:1].strip(), '{:.8f}'.format(float(s[1][33:43:1])), float(s[2][8:16:1]),
                            float(s[2][17:25:1]), fix_eccent(float(s[2][26:33:1])), float(s[2][34:42:1]),
                            float(s[2][43:51:1]), float(s[2][52:63:1])]])
        df_temp = pd.DataFrame(values, columns=col_names, index=['1'])
        df = df.append(df_temp, ignore_index=True)
        del df_temp


# --------------------------------------------Writing New CSV File------------------------------------------------------
# This writes a CSV file with the selected data
# Data can be added or taken away using df.drop('name_of_col' <- can be written as a list)
with open('sat_dat_out.csv', 'w', newline="") as writer:
    df.to_csv(writer, index=False)

print('New file saved as: sat_dat_out.csv')
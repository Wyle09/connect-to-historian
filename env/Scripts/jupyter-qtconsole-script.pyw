#!C:/My-Projects/Python/Connect-To-Historian/env\pythonw.exe
# EASY-INSTALL-ENTRY-SCRIPT: 'qtconsole==4.4.3','gui_scripts','jupyter-qtconsole'
__requires__ = 'qtconsole==4.4.3'
import re
import sys
from pkg_resources import load_entry_point

if __name__ == '__main__':
    sys.argv[0] = re.sub(r'(-script\.pyw?|\.exe)?$', '', sys.argv[0])
    sys.exit(
        load_entry_point('qtconsole==4.4.3', 'gui_scripts', 'jupyter-qtconsole')()
    )

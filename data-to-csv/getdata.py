import pandas
import pyodbc
import time


def sql_server_connection():
    server = "WIN-UIH9QHSEMDC"
    db = "Runtime"
    uid = "sa"
    pwd = "Password_1"
    conn = pyodbc.connect(driver='{SQL Server}', host=server, database=db,
                          user=uid, password=pwd)
    return conn


def get_file_name(file_path):
    time_str = time.strftime("%Y%m%d-%H%M%S")
    full_path = "{0}{1}{2}{3}".format(file_path, "testing-", time_str, ".csv")
    return full_path


def get_query_file(sql_file_path):
    query = open(sql_file_path, "r")
    return query.read()


def query_to_df(query_file):
    conn = sql_server_connection()
    df = pandas.io.sql.read_sql(query_file, conn)
    conn.commit()
    conn.close()
    return df


def create_csv_file(dataframe, csv_file_path):
    if not dataframe.empty:
        dataframe.to_csv(get_file_name(csv_file_path), index=False)


def main():
    # Need the complete file path of the sql file.
    sql_file_path = "C:\\my-projects\\python\\connect-to-historian\\" \
                    "_queries\\query-widehistory.sql"

    # Need the path where the csv file will be created.
    csv_file_path = "C:\\Users\\wyle.cordero\\Desktop\\"
    df = query_to_df(get_query_file(sql_file_path))
    create_csv_file(df, csv_file_path)


main()

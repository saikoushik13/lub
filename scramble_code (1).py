import datetime
import re
from pyspark.sql.functions import *
from pyspark.sql.types import *
from pyspark.sql.window import Window
from pyspark.sql import SparkSession
# modify the above imports as required and avoid using import *

spark = SparkSession.builder.appName("Spark Assignment").getOrCreate()

def read_data_and_create_dataframe():
    pass

def calculate_total_revenue_per_user():
    pass

def identify_top_ten_products():
    pass

def calculate_revenue_vs_coupon_usage():
    pass

def find_top_three_products_per_user():
    pass

def identify_inactive_users():
    pass

def identify_most_valuable_users():
    pass

def create_df_with_masked_email_id():
    pass

def main():
    (sales_df, events_df, products_df, users_df) = read_data_and_create_dataframe()

    total_revenue_per_user_df = calculate_total_revenue_per_user()
    total_revenue_per_user_df.show(truncate=False)

    top_ten_products_df = identify_top_ten_products()
    top_ten_products_df.show(truncate=False)

    revenue_vs_coupon_usage_df = calculate_revenue_vs_coupon_usage()
    revenue_vs_coupon_usage_df.show(truncate=False)

    top_three_pdt_per_user_df = find_top_three_products_per_user()
    top_three_pdt_per_user_df.show(truncate=False)

    inactive_users_df = identify_inactive_users()
    inactive_users_df.show(truncate=False)

    most_valuable_users_df = identify_most_valuable_users()
    most_valuable_users_df.show(truncate=False)

    users_data_with_masked_email_df = create_df_with_masked_email_id()
    users_data_with_masked_email_df.show(truncate=False)

if __name__ == "__main__":
    main()
    spark.stop()
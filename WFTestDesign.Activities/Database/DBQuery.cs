using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;
using System.Data;
using System.Data.SqlClient;
using WFTestDesign.Activities.Helpers;

namespace WFTestDesign.Activities.Database
{

    public sealed class DBQuery : CodeActivity
    {
        #region Declarations
        // Define an activity input argument of type string
        public String ConnectionString { get; set; }
        public String SQLQuery { get; set; }
        public int DelayBeforeCheck { get; set; }
        public int NumberOfRowsExpected { get; set; }

        #endregion

        #region Implementation
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            //string text = context.GetValue(this.Text);

            string sqlQueryToExecute = SQLQuery;

            // Sleep for delay seconds...
            if (0 < DelayBeforeCheck)
            {
                Logger.TestStepDetail("Sleeping for {0} seconds", DelayBeforeCheck);
                System.Threading.Thread.Sleep(DelayBeforeCheck * 1000);
            }

            Logger.TestStepDetail("Executing database query: {0}", sqlQueryToExecute);
            DataSet ds = FillDataSet(ConnectionString, sqlQueryToExecute);

            if (ds.Tables[0].Rows.Count != NumberOfRowsExpected)
            {
                Logger.TestStepDetail("Number of rows expected to be returned by the query does not match the value specified in the teststep. NumberOfRowsExpected were: {0}, actual: {1}", NumberOfRowsExpected, ds.Tables[0].Rows.Count);
                throw new WFTestExceptions("Number of rows expected to be returned by the query does not match the value specified in the teststep. NumberOfRowsExpected were: {0}, actual: {1}", NumberOfRowsExpected, ds.Tables[0].Rows.Count);
            }
            if (ds.Tables[0].Rows.Count == NumberOfRowsExpected)
            {
                Logger.TestStepDetail("Number of rows are good: {0}, actual: {1}", NumberOfRowsExpected, ds.Tables[0].Rows.Count);
            }

        }

        #endregion

        #region Metadata
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }

        static DBQuery()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(
                typeof(DBQuery),
                "ConnectionString",
                PropertyValueEditor.CreateEditorAttribute(typeof(UI.DataBaseConnectionDialog)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
       
        }

        private static DataSet FillDataSet(string connectionString, string sqlQuery)
        {
            var connection = new SqlConnection(connectionString);
            var ds = new DataSet();

            using (connection)
            {
                var adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(sqlQuery, connection)
                };
                adapter.Fill(ds);
                return ds;
            }
        }
        #endregion
    }
}

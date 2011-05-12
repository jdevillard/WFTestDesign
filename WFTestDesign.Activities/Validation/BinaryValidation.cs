using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.IO;
using WFTestDesign.Activities.Helpers;

namespace WFTestDesign.Activities.Validation
{

    public sealed class BinaryValidationStep : CodeActivity
    {

        #region declarations

        // Define an activity input argument of type string
        public InArgument<string> ValidationFilePath { get; set; }
        private bool compareAsUTF8 { get; set; }

        [Browsable(false)]
        public MemoryStream StreamToValidate { get; set; }

        [Description("Choose if you want to compare the file as UTF8")]
        public bool CompareAsUTF8
        {
            get
            {
                return this.compareAsUTF8;
            }
            set
            {
                this.compareAsUTF8 = value;
            }
        }

        #endregion
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            string validationFilePath = context.GetValue(this.ValidationFilePath);

            MemoryStream data = StreamToValidate;
            MemoryStream dataToValidateAgainst = null;

            try
            {
                try
                {
                    Logger.TestStepDetail("Load file {0} to Stream ",validationFilePath);
                    dataToValidateAgainst = Helpers.StreamHelper.LoadFileToStream(validationFilePath);

                }
                catch (Exception e)
                {
                    //LogError("BinaryValidationStep failed, exception caugh trying to open comparison file: {0}", this.comparisonDataPath);
                    throw new ApplicationException(string.Format("BinaryValidationStep failed, exception caugh trying to open comparison file: {0}", validationFilePath));
                }

                try
                {
                    data.Seek(0, SeekOrigin.Begin);
                    dataToValidateAgainst.Seek(0, SeekOrigin.Begin);

                    if (this.compareAsUTF8)
                    {
                        // Compare the streams, make sure we are comparing like for like
                        Logger.TestStepDetail("Compare the two streams as UTF8");
                        StreamHelper.CompareStreams(StreamHelper.EncodeStream(data, System.Text.Encoding.UTF8), StreamHelper.EncodeStream(dataToValidateAgainst, System.Text.Encoding.UTF8));

                        //If not catched so compare result as equal stream
                        Logger.TestStepDetail("Test succeed -- Stream are identical");
                    }
                    else
                    {
                        Logger.TestStepDetail("Compare the two streams");
                        StreamHelper.CompareStreams(data, dataToValidateAgainst);
                        //If not catched so compare result as equal stream
                        Logger.TestStepDetail("Test succeed -- Stream are identical");
                    }
                }
                catch (Exception e)
                {
                    //LogError("Binary validation failed while comparing the two data streams with the following exception: {0}", e.ToString());

                    // Dump out streams for validation...
                    data.Seek(0, SeekOrigin.Begin);
                    dataToValidateAgainst.Seek(0, SeekOrigin.Begin);
                    //LogData("Stream 1:", data);
                    //LogData("Stream 2:", dataToValidateAgainst);

                    throw;
                }
            }
            finally
            {
                if (null != dataToValidateAgainst)
                {
                    dataToValidateAgainst.Close();
                }
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;
using WFTestDesign.Activities.Helpers;


namespace WFTestDesign.Activities
{
    [Designer(typeof(WFTestDesign.Activities.Designers.MySequenceDesigner))]
   // public sealed class TestCase : NativeActivity
    public  class TestCase : NativeActivity
    {

        #region Declarations

        public readonly Collection<Activity> m_SetupActivities;
        public readonly Collection<Activity> m_Activities;
        public readonly Collection<Activity> m_CleanUpActivities;

        private Int32 m_Index;
        private string m_Stage;
        private Exception m_StageException;

        private CompletionCallback m_CompletionCallback;
        private FaultCallback m_FaultCallback;
        //private Helpers.Logger logger;

        private Variable<string> var_String;

        #endregion

        #region Properties
        
        ///
        /// Scope Variable
        ///
        Collection<Variable> variables;
        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get
            {
                if (this.variables == null)
                {
                    variables = new Collection<Variable>();
                }
                return variables;
            }

        }


        /// 
        /// Collection d'activités de la séquence
        /// 
        [Browsable(false)]
        [DependsOn("SetupActivities")]
        public Collection<Activity> Activities
        {
            get { return this.m_Activities; }
        }

        [Browsable(false)]
        [DependsOn("Variables")]
        public Collection<Activity> SetupActivities
        {
            get { return this.m_SetupActivities; }
        }
        [Browsable(false)]
        [DependsOn("Activities")]
        public Collection<Activity> CleanUpActivities
        {
            get { return this.m_CleanUpActivities; }
        }
        
        #endregion
        
        
        
        /// 
        /// Constructeur
        /// 
        public TestCase()
        {
            
            this.m_SetupActivities = new Collection<Activity>();
            this.m_Activities = new Collection<Activity>();
            this.m_CleanUpActivities = new Collection<Activity>();
            this.var_String = new Variable<string>();
        }

        

        /// 
        /// Execute
        /// 
        /// WF context
        /// 
        protected override void Execute(NativeActivityContext context)
        {
            // Initialisation
            this.m_Index = 0;
            this.m_Stage = "Setup";

            
            this.var_String.Set(context,"Test de string");

            //Log Initialisation
            //this.logger = new Helpers.Logger();
            Logger.TestStageStart(this.m_Stage, DateTime.Now);
            


            // Caching callback delegates
            this.m_CompletionCallback = new CompletionCallback(this.MyCompletionCallback);
            this.m_FaultCallback = new FaultCallback(this.MyFaultCallback);

            // Schedule next activity
            this.ScheduleNextSetup(context);
            //this.ScheduleNext(context);
            
        }

        /// 
        /// Schedule next activity in activities collection
        /// 
        /// 
        private void ScheduleNext(NativeActivityContext context)
        {

            if (m_Index == 0)
            {
                this.m_Stage = "Execution";
                Logger.TestStageStart(this.m_Stage, DateTime.Now);
            }

            if (this.m_Index < this.m_Activities.Count)
            {
                Logger.TestStepStart(this.m_Activities[this.m_Index].DisplayName);

                context.ScheduleActivity(
                    this.m_Activities[this.m_Index],
                    this.m_CompletionCallback,
                    this.m_FaultCallback);

                this.m_Index++;
            }
            
        }

        private void ScheduleNextSetup(NativeActivityContext context)
        {
            //If no activities goto the Next Step
            if (this.m_SetupActivities.Count == 0)
                this.ScheduleNext(context);

            if (this.m_Index < this.m_SetupActivities.Count)
            {
                Logger.TestStepStart(this.m_SetupActivities[this.m_Index].DisplayName);

                context.ScheduleActivity(
                    this.m_SetupActivities[this.m_Index],
                    this.m_CompletionCallback,
                    this.m_FaultCallback);

                this.m_Index++;
            }

        }

        private void ScheduleNextCleanUp(NativeActivityContext context)
        {
            if (this.m_Index == 0)
            {
                this.m_Stage = "CleanUp";
                Logger.TestStageStart(this.m_Stage, DateTime.Now);
            }

            if (this.m_Index < this.m_CleanUpActivities.Count)
            {
                Logger.TestStepStart(this.m_CleanUpActivities[this.m_Index].DisplayName);

                context.ScheduleActivity(
                    this.m_CleanUpActivities[this.m_Index],
                    this.m_CompletionCallback,
                    this.m_FaultCallback);

                this.m_Index++;
            }
            else
            {
                //If no activity defined in the cleanup Stage.
                Logger.TestStageEnd(m_Stage, DateTime.Now, null);
                if (m_StageException != null)
                    throw m_StageException;
            }

        }

        public void MyCompletionCallback(NativeActivityContext context, ActivityInstance completedInstance)
        {
            //Understand why Faulted activity goes in the completionCallBack
            if (completedInstance.State.ToString()=="Closed")
            {
                switch (m_Stage)
                {
                    case "Setup":
                        Logger.TestStepEnd(completedInstance.Activity.DisplayName, DateTime.Now, null);
                        if (this.m_Index >= m_SetupActivities.Count)
                        {
                            m_Index = 0;
                            Logger.TestStageEnd(m_Stage, DateTime.Now, null);
                            ScheduleNext(context);
                        }
                        else
                            this.ScheduleNextSetup(context);
                        break;
                    case "Execution":
                        Logger.TestStepEnd(completedInstance.Activity.DisplayName, DateTime.Now, null);
                        if (this.m_Index >= m_Activities.Count)
                        {
                            this.m_Index = 0;
                            Logger.TestStageEnd(m_Stage, DateTime.Now, null);
                            ScheduleNextCleanUp(context);
                        }
                        else
                            this.ScheduleNext(context);
                        break;
                    case "CleanUp":
                        Logger.TestStepEnd(completedInstance.Activity.DisplayName, DateTime.Now, null);
                        if (this.m_Index >= m_CleanUpActivities.Count)
                        {
                            Logger.TestStageEnd(m_Stage, DateTime.Now, null);
                            if (m_StageException != null)
                                throw m_StageException;
                        }
                        else
                            this.ScheduleNextCleanUp(context);

                        break;
                }
            }


            
            
        }

        public void MyFaultCallback(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            switch (m_Stage)
            {
                case "Setup":
                    Logger.TestStepEnd(this.SetupActivities[this.m_Index-1].DisplayName, DateTime.Now, propagatedException);
                    Logger.TestStageEnd("Setup", DateTime.Now, propagatedException);
                        //throw propagatedException;
                    m_Index = 0;
                    m_StageException = propagatedException;
                    faultContext.HandleFault();
                    faultContext.CancelChild(propagatedFrom);
                    this.ScheduleNextCleanUp(faultContext);
                    break;
                case "Execution":
                        Logger.TestStepEnd(this.m_Activities[this.m_Index-1].DisplayName, DateTime.Now, propagatedException);
                        Logger.TestStageEnd("Execution", DateTime.Now, propagatedException);
                        //throw propagatedException;
                        m_Index = 0;
                        m_StageException = propagatedException;
                        faultContext.HandleFault();
                        faultContext.CancelChild(propagatedFrom);
                    
                        this.ScheduleNextCleanUp(faultContext);
                    break;
                case "CleanUp":
                        Logger.TestStepEnd(this.m_CleanUpActivities[this.m_Index-1].DisplayName, DateTime.Now, propagatedException);
                        Logger.TestStageEnd("CleanUp", DateTime.Now, propagatedException);
                        throw propagatedException;
                    break;

            }


        }


        /// 
        /// Register activity's metadata
        /// 
        /// 
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            //Adding Setup Activities to metadata
            if (this.m_SetupActivities != null
                && this.m_SetupActivities.Count != 0)
            {
                foreach (Activity a in this.m_SetupActivities)
                {
                    metadata.AddChild(a);
                }
            }
            

            //Adding Execution Activities to metadata
            if (this.m_Activities != null
                && this.m_Activities.Count != 0)
            {
                foreach (Activity a in this.m_Activities)
                {
                    metadata.AddChild(a);
                }
            }

            //Adding CleanUp Activities to metadata
            if (this.m_CleanUpActivities != null
                && this.m_CleanUpActivities.Count != 0)
            {
                foreach (Activity a in this.m_CleanUpActivities)
                {
                    metadata.AddChild(a);
                }
            }

            metadata.AddImplementationVariable(var_String);

            if (variables != null)
            {
                foreach (Variable item in variables)
                {
                    metadata.AddImplementationVariable(item);
                }
            }
        }

    }
}

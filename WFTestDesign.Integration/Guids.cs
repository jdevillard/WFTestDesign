// Guids.cs
// MUST match guids.h
using System;

namespace Microsoft.WFTestDesign_Integration
{
    static class GuidList
    {
        public const string guidWFTestDesign_IntegrationPkgString = "a51bcdc4-108b-4ce4-bf3e-e1b1dbfc159e";
        public const string guidWFTestDesign_IntegrationCmdSetString = "01b74a11-0da3-4473-9e56-f17f68beff0e";

        public static readonly Guid guidWFTestDesign_IntegrationCmdSet = new Guid(guidWFTestDesign_IntegrationCmdSetString);
    };

    static class version
    {
        public const string fileversion = "0.6";
    }
}
﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using Infrastructure.Common;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Xunit;

public partial class CustomBindingTests : ConditionalWcfTest
{
    // Http: Client and Server bindings setup exactly the same using default settings.
    [Fact]
    [OuterLoop]
    public static void DefaultSettings_Http_Text_Echo_RoundTrips_String()
    {
        string testString = "Hello";
        CustomBinding binding = null;
        ChannelFactory<IWcfService> factory = null;
        IWcfService serviceProxy = null;

        try
        {
            // *** SETUP *** \\
            binding = new CustomBinding(new TextMessageEncodingBindingElement(), new HttpTransportBindingElement());
            factory = new ChannelFactory<IWcfService>(binding, new EndpointAddress(Endpoints.DefaultCustomHttp_Address));
            serviceProxy = factory.CreateChannel();

            // *** EXECUTE *** \\
            string result = serviceProxy.Echo(testString);

            // *** VALIDATE *** \\
            Assert.NotNull(result);
            Assert.Equal(testString, result);

            // *** CLEANUP *** \\
            factory.Close();
            ((ICommunicationObject)serviceProxy).Close();
        }
        finally
        {
            // *** ENSURE CLEANUP *** \\
            ScenarioTestHelpers.CloseCommunicationObjects((ICommunicationObject)serviceProxy, factory);
        }
    }

    // Https: Client and Server bindings setup exactly the same using default settings.
#if FULLXUNIT_NOTSUPPORTED
    [Fact]
#else
    [ConditionalFact(nameof(Root_Certificate_Installed))]
    [ActiveIssue(1123, PlatformID.AnyUnix)]
#endif
    [OuterLoop]
    public static void DefaultSettings_Https_Text_Echo_RoundTrips_String()
    {
#if FULLXUNIT_NOTSUPPORTED
        bool root_Certificate_Installed = Root_Certificate_Installed();
        if (!root_Certificate_Installed)
        {
            Console.WriteLine("---- Test SKIPPED --------------");
            Console.WriteLine("Attempting to run the test in ToF, a ConditionalFact evaluated as FALSE.");
            Console.WriteLine("Root_Certificate_Installed evaluated as {0}", root_Certificate_Installed);
            return;
        }
#endif
        string testString = "Hello";
        CustomBinding binding = null;
        ChannelFactory<IWcfService> factory = null;
        IWcfService serviceProxy = null;

        try
        {
            // *** SETUP *** \\
            binding = new CustomBinding(new TextMessageEncodingBindingElement(), new HttpsTransportBindingElement());
            factory = new ChannelFactory<IWcfService>(binding, new EndpointAddress(Endpoints.HttpsSoap12_Address));
            serviceProxy = factory.CreateChannel();

            // *** EXECUTE *** \\
            string result = serviceProxy.Echo(testString);

            // *** VALIDATE *** \\
            Assert.NotNull(result);
            Assert.Equal(testString, result);

            // *** CLEANUP *** \\
            factory.Close();
            ((ICommunicationObject)serviceProxy).Close();
        }
        finally
        {
            // *** ENSURE CLEANUP *** \\
            ScenarioTestHelpers.CloseCommunicationObjects((ICommunicationObject)serviceProxy, factory);
        }
    }
}
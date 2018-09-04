﻿// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace WpfClient.Shell.CefInfrastructure.Proxy
{
    public struct InternetProxyInfo
    {
        public InternetOpenType AccessType;
        public string ProxyAddress;
        public string ProxyBypass;
    }
}
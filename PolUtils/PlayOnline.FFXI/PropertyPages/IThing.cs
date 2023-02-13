// Copyright � 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PlayOnline.FFXI.PropertyPages
{
    public partial class IThing : UserControl
    {
        public IThing() { this.InitializeComponent(); }

        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public string TabName
        {
            get { return this.TabName_; }
            set { this.TabName_ = value; }
        }

        private string TabName_ = String.Empty;

        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool IsFixedSize
        {
            get { return this.IsFixedSize_; }
            set { this.IsFixedSize_ = value; }
        }

        private bool IsFixedSize_ = false;
    }
}

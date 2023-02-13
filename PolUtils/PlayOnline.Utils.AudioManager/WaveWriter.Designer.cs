// Copyright � 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.Utils.AudioManager {

  public partial class WaveWriter {

    #region Controls

    private System.Windows.Forms.Label lblSource;
    private System.Windows.Forms.Label lblTarget;
    private System.Windows.Forms.TextBox txtSource;
    private System.Windows.Forms.TextBox txtTarget;
    private System.Windows.Forms.ProgressBar prbBytesWritten;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaveWriter));
			this.prbBytesWritten = new System.Windows.Forms.ProgressBar();
			this.lblSource = new System.Windows.Forms.Label();
			this.lblTarget = new System.Windows.Forms.Label();
			this.txtSource = new System.Windows.Forms.TextBox();
			this.txtTarget = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// prbBytesWritten
			// 
			resources.ApplyResources(this.prbBytesWritten, "prbBytesWritten");
			this.prbBytesWritten.Name = "prbBytesWritten";
			// 
			// lblSource
			// 
			this.lblSource.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.lblSource, "lblSource");
			this.lblSource.Name = "lblSource";
			// 
			// lblTarget
			// 
			this.lblTarget.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.lblTarget, "lblTarget");
			this.lblTarget.Name = "lblTarget";
			// 
			// txtSource
			// 
			resources.ApplyResources(this.txtSource, "txtSource");
			this.txtSource.Name = "txtSource";
			this.txtSource.ReadOnly = true;
			// 
			// txtTarget
			// 
			resources.ApplyResources(this.txtTarget, "txtTarget");
			this.txtTarget.Name = "txtTarget";
			this.txtTarget.ReadOnly = true;
			// 
			// WaveWriter
			// 
			resources.ApplyResources(this, "$this");
			this.ControlBox = false;
			this.Controls.Add(this.txtTarget);
			this.Controls.Add(this.txtSource);
			this.Controls.Add(this.lblTarget);
			this.Controls.Add(this.lblSource);
			this.Controls.Add(this.prbBytesWritten);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "WaveWriter";
			this.ShowInTaskbar = false;
			this.Activated += new System.EventHandler(this.WaveWriter_Activated);
			this.Load += new System.EventHandler(this.WaveWriter_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

    }

    #endregion

  }

}

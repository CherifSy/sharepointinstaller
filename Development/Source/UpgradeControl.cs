/**********************************************************************/
/*                                                                    */
/*                   SharePoint Solution Installer                    */
/*             http://www.codeplex.com/sharepointinstaller            */
/*                                                                    */
/*               (c) Copyright 2007 Lars Fastrup Nielsen.             */
/*                                                                    */
/*  This source is subject to the Microsoft Permissive License.       */
/*  http://www.codeplex.com/sharepointinstaller/Project/License.aspx  */
/*                                                                    */
/* KML: Updated to use Operation now owned by InstallerForm           */
/*                                                                    */
/**********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CodePlex.SharePointInstaller
{
  public partial class UpgradeControl : InstallerControl
  {

    public UpgradeControl(bool bUpgrade)
    {
      InitializeComponent();
      if (!bUpgrade)
      {
        this.upgradeRadioButton.Checked = false;
      }


      messageLabel.Text = InstallConfiguration.FormatString(messageLabel.Text);

      string upgradeDescription = InstallConfiguration.UpgradeDescription;
      if (upgradeDescription != null)
      {
        upgradeDescriptionLabel.Text = upgradeDescription;
      }
    }

    protected internal override void Open(InstallOptions options)
    {
      bool enable = upgradeRadioButton.Checked || removeRadioButton.Checked;
      Form.Operation = InstallOperation.Upgrade;
      Form.NextButton.Enabled = enable;
      doactivateFeaturesChoice.Checked = Form.WillActivateFeatures;
      dodeactivateFeaturesChoice.Checked = Form.WillDeactivateFeatures;
      UpdateButtons();
    }

    protected internal override void Close(InstallOptions options)
    {
    }

    private void upgradeRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (upgradeRadioButton.Checked)
      {
        Form.Operation = InstallOperation.Upgrade;
        Form.NextButton.Enabled = true;
        doactivateFeaturesChoice.Checked = Form.WillActivateFeatures;
        UpdateButtons();
      }
    }

    private void removeRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (removeRadioButton.Checked)
      {
        Form.Operation = InstallOperation.Uninstall;
        Form.NextButton.Enabled = true;
        UpdateButtons();
        // Clear visual check for activate, as it is not relevant now
        // but preserve underlying setting, in case user reenables it
        bool activpref = Form.WillActivateFeatures;
        doactivateFeaturesChoice.Checked = false;
        Form.WillActivateFeatures = activpref;
      }
    }

    private void dodeactivateFeaturesChoice_CheckedChanged(object sender, EventArgs e)
    {
        Form.WillDeactivateFeatures = dodeactivateFeaturesChoice.Checked;
        UpdateButtons();
    }

    private void doactivateFeaturesChoice_CheckedChanged(object sender, EventArgs e)
    {
        Form.WillActivateFeatures = doactivateFeaturesChoice.Checked;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        doactivateFeaturesChoice.Enabled = upgradeRadioButton.Checked && dodeactivateFeaturesChoice.Checked;
    }
  }
}

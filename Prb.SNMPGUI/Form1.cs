using System;

using System.Windows.Forms;
using Prb.SNMPServices;



namespace Prb.SNMPGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SNMPOperations obj = new SNMPOperations();
            lstResults.Text = obj.SNMPGetRequest(txtCommunity.Text, txtIPAddress.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SNMPGetBulk obj = new SNMPGetBulk();
            //lstResults.Text = 
           // obj.SNMPWalkGetBulk(txtCommunity.Text, txtIPAddress.Text);
            //SNMPWalkGetNext
            obj.SNMPWalkGetNext(txtCommunity.Text, txtIPAddress.Text);
        }

        private void txtIPAddress_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

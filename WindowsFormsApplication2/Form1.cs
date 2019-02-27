using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;//LINQ TO XML

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        XElement xe;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var xenew = XElement.Load(openFileDialog1.FileName);
                foreach (XElement xn in xenew.Elements("book"))
                {
                    var xo = from x in xe.Elements("book")
                             where x.Element("book").Value.Equals(xn.Element("book_id").Value)
                             select x;
                    if (xo.Count() > 0)
                    {
                        if (MessageBox.Show(xn.Element("book_id").Value + "重複，確認覆蓋嗎?", "XML瀏覽器",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            xo.First().ReplaceWith(xn);
                        }
                    }
                    else
                    {
                        xe.Add(xn);



                    }
                }
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                var trn = new TreeNode(xe.Name.ToString());
                treeView1.Nodes.Add(trn);
                AddNode(xe.Elements(), trn);
                treeView1.EndUpdate();
                treeView1.ExpandAll();
            }
        }
        

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //開檔案對話窗
            {
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                xe = XElement.Load(openFileDialog1.FileName);
                var trn = new TreeNode(xe.Name.ToString());
                treeView1.Nodes.Add(trn);
                AddNode(xe.Elements(), trn);
                treeView1.EndUpdate();
                treeView1.ExpandAll();

                Down.Enabled = true;
                Up.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button6.Enabled = true;

            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        private void AddNode(IEnumerable<XElement> xes, TreeNode treeNode)
        {
            foreach (XElement xn in xes)
            {
                var tn = new TreeNode(xn.Name.ToString());
                treeNode.Nodes.Add(tn);

                if (xn.Elements().Count() > 0)
                {
                    AddNode(xn.Elements(), tn);

                }
                else
                {
                    tn.Nodes.Add(xn.Value.ToString());
                }

            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void Up_Click(object sender, EventArgs e)
        {
            var tn = treeView1.SelectedNode;
            if(tn != null && tn.Parent != null)
            {
                while (tn.Text != "book")
                {
                    tn = tn.Parent;

                }
                var pn = tn.PrevNode;
                if(pn != null)
                {
                    treeView1.BeginUpdate();
                    tn.Parent.Nodes.Remove(pn);
                    tn.Parent.Nodes.Insert(tn.Parent.Nodes.IndexOf(tn) + 1, pn);
                    treeView1.EndUpdate();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Down_Click(object sender, EventArgs e)
        {
            var tn = treeView1.SelectedNode;
            if (tn != null && tn.Parent != null)
            {
                while (tn.Text != "book")
                {
                    tn = tn.Parent;

                }
                var pn = tn.NextNode;
                if (pn != null)
                {
                    treeView1.BeginUpdate();
                    tn.Parent.Nodes.Remove(pn);
                    tn.Parent.Nodes.Insert(tn.Parent.Nodes.IndexOf(tn) - 1, pn);
                    treeView1.EndUpdate();
                }
            }
        }
    }
}

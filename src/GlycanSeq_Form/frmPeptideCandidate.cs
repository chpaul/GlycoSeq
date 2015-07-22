using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COL.GlycoSequence;

namespace GlycanSeq_Form
{
    public partial class frmPeptideCandidate : Form
    {
        private List<TargetPeptide> _lstPeptides;
        private DataTable dtPeptide;
        public frmPeptideCandidate(ref List<TargetPeptide> argTargetPeptides)
        {
            InitializeComponent();
            _lstPeptides = argTargetPeptides;
            ShowDataGrid();
            dgvPeptide.Columns[0].Width = 250;
            dgvPeptide.Columns[1].Width = 80;
            dgvPeptide.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPeptide.Columns[2].Width =100;
            dgvPeptide.Columns[3].Width = 40;
            dgvPeptide.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPeptide.Columns[4].Width = 40;
            dgvPeptide.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPeptide.Columns[5].Width = 80;
            dgvPeptide.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPeptide.Columns[6].Width = 90;
            dgvPeptide.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPeptide.Columns[7].Width = 270;
        }

        private void ShowDataGrid()
        {
            dtPeptide = new DataTable();
            dtPeptide.Columns.Add("Peptide Sequence*", typeof(string));
            dtPeptide.Columns.Add("Peptide Mass*", typeof(float));
            dtPeptide.Columns.Add("Protein Name", typeof(string));
            dtPeptide.Columns.Add("N Term", typeof(string));
            dtPeptide.Columns.Add("C Term", typeof(string));
            dtPeptide.Columns.Add("Start Time", typeof(float));
            dtPeptide.Columns.Add("End Time", typeof(float));
            dtPeptide.Columns.Add("Modification", typeof(string));
            foreach (TargetPeptide tPeptide in _lstPeptides)
            {
                DataRow row = dtPeptide.NewRow();
                row[0] = tPeptide.PeptideSequence;
                row[1] = tPeptide.PeptideMass;
                row[2] = tPeptide.ProteinName;
                row[3] = tPeptide.AminoAcidBefore;
                row[4] = tPeptide.AminoAcidAfter;
                row[5] = tPeptide.StartTime;
                row[6] = tPeptide.EndTime;

                string Mod = "";
                foreach (var modKey in tPeptide.Modifications.Keys)
                {
                    Mod += modKey + " * " + tPeptide.Modifications[modKey] + " ;";
                }
                row[7] = Mod;
                dtPeptide.Rows.Add(row);
            }
            dgvPeptide.DataSource = dtPeptide;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //Check all peptide sequence and peptide mass cell

            for(int i =0;i<dtPeptide.Rows.Count;i++)
            {
                if (dtPeptide.Rows[i][0].ToString() == "")
                {
                    MessageBox.Show("Peptide Sequence can't be empty @Row " + (i+1).ToString());
                    dgvPeptide.CurrentCell = dgvPeptide.Rows[i].Cells[0];
                    return;
                }
                if (dtPeptide.Rows[i][1].ToString() == "")
                {
                    MessageBox.Show("Peptide mass can't be empty @Row " + (i + 1).ToString());
                    dgvPeptide.CurrentCell = dgvPeptide.Rows[i].Cells[1];
                    return;
                }
            }

            _lstPeptides.Clear();
            foreach (DataRow dRow in dtPeptide.Rows)
            {
                TargetPeptide tPeptide = new TargetPeptide(dRow[0].ToString(), dRow[2].ToString(), Convert.ToSingle(dRow[1]), Convert.ToSingle(dRow[5]), Convert.ToSingle(dRow[6]));
                tPeptide.AminoAcidBefore = dRow[3].ToString();
                tPeptide.AminoAcidAfter = dRow[4].ToString();
                string[] ModArray = dRow[7].ToString().Split(';');
                for (int i = 0; i < ModArray.Length; i++)
                {
                    if (ModArray[i] != "")
                    {
                        string ModType = ModArray[i].Split('*')[0].TrimEnd();
                        int ModCount = Convert.ToInt32(ModArray[i].Split('*')[1].TrimStart());
                        tPeptide.Modifications.Add(ModType, ModCount);
                    }
                }
                _lstPeptides.Add(tPeptide);
            }
            MessageBox.Show("Total " + _lstPeptides.Count.ToString() + "  glycopeptides saved");
        }

        private void dgvPeptide_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                float tmpFloat = 0;
                if (float.TryParse(dgvPeptide[1, e.RowIndex].EditedFormattedValue.ToString(), out tmpFloat))
                {
                    dgvPeptide[e.ColumnIndex, e.RowIndex].ErrorText = string.Empty;
                }
                if (dgvPeptide[1, e.RowIndex].EditedFormattedValue.ToString() == "")
                {
                    dgvPeptide[e.ColumnIndex, e.RowIndex].ErrorText = "Peptide mass cannot be empty";
                    MessageBox.Show("Peptide mass cannot be empty @Row " + (e.RowIndex + 1).ToString());
                    e.Cancel = true;
                }
                else
                {
                    dgvPeptide[e.ColumnIndex, e.RowIndex].ErrorText = string.Empty;
                }
            }
            if (e.ColumnIndex == 0)
            {
                if (dgvPeptide[0, e.RowIndex].EditedFormattedValue.ToString() == "")
                {
                    dgvPeptide[e.ColumnIndex, e.RowIndex].ErrorText = "Peptide sequence cannot be empty";
                    MessageBox.Show("Peptide sequence cannot be empty @Row " + (e.RowIndex + 1).ToString());
                    e.Cancel = true;
                }
                else
                {
                    dgvPeptide[e.ColumnIndex, e.RowIndex].ErrorText = string.Empty;
                }
            }
        }

        private void dgvPeptide_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                float tmpFloat = 0;
                if (!float.TryParse(dgvPeptide[1,e.RowIndex].EditedFormattedValue.ToString(), out tmpFloat))
                {
                    dgvPeptide[e.ColumnIndex, e.RowIndex].ErrorText = "Peptide mass can only be numeric";
                    MessageBox.Show("Peptide mass can only be numeric @Row " + (e.RowIndex + 1).ToString());
                    e.Cancel = true;
                }
            }
        }

  
    }
}

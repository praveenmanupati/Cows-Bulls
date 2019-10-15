using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Cows_and_Bulls
{
    public class WordsArray : System.Collections.CollectionBase
    {
        private readonly Form HostForm;

        public Label AddRow()
        {
            Label aLabel = new Label(); this.List.Add(aLabel); HostForm.Controls.Add(aLabel); aLabel.Top = 30 + Count * 30; aLabel.Left = 23;
            aLabel.Tag = "dynamic1" + this.Count; aLabel.Size = new Size(37, 20); aLabel.AutoSize = true; //aLabel.Text = "Machine " + aLabel.Tag.ToString();
            //aLabel.BackColor = Color.Yellow; //FontFamily family = new FontFamily("Microsoft Sans Serif");
            //aLabel.Font = new Font(aLabel.Font, FontStyle.Bold);
            aLabel.Font = new Font(new FontFamily("Microsoft Sans Serif") , 12.0f); 
            aLabel.TextAlign = ContentAlignment.MiddleCenter;
            return aLabel;
        }

        public WordsArray(Form host)
        {
            HostForm = host;
            this.AddRow();
        }

        public Label this[int Index]
        {
            get
            {
                return (Label)this.List[Index];
            }
        }

        public void Remove()
        {
            if (this.Count > 0)
            {
                HostForm.Controls.Remove(this[0]);
                this.List.RemoveAt(0);
            }
        }
    }

    public class BullsArray : System.Collections.CollectionBase
    {
        private readonly Form HostForm;

        public Label AddRow()
        {
            Label aLabel = new Label(); this.List.Add(aLabel); HostForm.Controls.Add(aLabel); aLabel.Top = 30 + Count * 30; aLabel.Left = 101;
            aLabel.Tag = "dynamic2" + this.Count; aLabel.Size = new Size(18, 20); aLabel.AutoSize = true; 
            aLabel.Font = new Font(new FontFamily("Microsoft Sans Serif"), 12.0f);
            aLabel.TextAlign = ContentAlignment.MiddleCenter;
            return aLabel;
        }

        public BullsArray(Form host)
        {
            HostForm = host;
            this.AddRow();
        }

        public Label this[int Index]
        {
            get
            {
                return (Label)this.List[Index];
            }
        }

        public void Remove()
        {
            if (this.Count > 0)
            {
                HostForm.Controls.Remove(this[0]);
                this.List.RemoveAt(0);
            }
        }
    }

    public class CowsArray : System.Collections.CollectionBase
    {
        private readonly Form HostForm;

        public Label AddRow()
        {
            Label aLabel = new Label(); this.List.Add(aLabel); HostForm.Controls.Add(aLabel); aLabel.Top = 30 + Count * 30; aLabel.Left = 165;
            aLabel.Tag = "dynamic3" + this.Count; aLabel.Size = new Size(18, 20); aLabel.AutoSize = true;
            aLabel.Font = new Font(new FontFamily("Microsoft Sans Serif"), 12.0f);
            aLabel.TextAlign = ContentAlignment.MiddleCenter;
            return aLabel;
        }

        public CowsArray(Form host)
        {
            HostForm = host;
            this.AddRow();
        }

        public Label this[int Index]
        {
            get
            {
                return (Label)this.List[Index];
            }
        }

        public void Remove()
        {
            if (this.Count > 0)
            {
                HostForm.Controls.Remove(this[0]);
                this.List.RemoveAt(0);
            }
        }
    }

    public class ChancesArray : System.Collections.CollectionBase
    {
        private readonly Form HostForm;

        public Label AddRow()
        {
            Label aLabel = new Label(); this.List.Add(aLabel); HostForm.Controls.Add(aLabel); aLabel.Top = 30 + Count * 30; aLabel.Left = 252;
            aLabel.Tag = "dynamic4" + this.Count; aLabel.Size = new Size(18, 20); aLabel.AutoSize = true;
            aLabel.Font = new Font(new FontFamily("Microsoft Sans Serif"), 12.0f);
            aLabel.TextAlign = ContentAlignment.MiddleCenter;
            return aLabel;
        }

        public ChancesArray(Form host)
        {
            HostForm = host;
            this.AddRow();
        }

        public Label this[int Index]
        {
            get
            {
                return (Label)this.List[Index];
            }
        }

        public void Remove()
        {
            if (this.Count > 0)
            {
                HostForm.Controls.Remove(this[0]);
                this.List.RemoveAt(0);
            }
        }
    }
}

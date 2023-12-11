﻿using gamon.TreeMptt;
using SchoolGrades;
using SchoolGrades.BusinessObjects;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolGrades_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmLessons : Window
    {
        bool isLoading = true;

        TreeMptt topicTreeMptt;

        Lesson currentLesson = new Lesson();

        Class currentClass;

        //List<Topic> listTopicsBefore;

        List<SchoolGrades.BusinessObjects.Image> listImages;
        private int indexImages = 0;

        private SchoolSubject currentSchoolSubject;

        bool isFormClosed = false;
        private int currentLessonsGridIndex;

        public bool IsFormClosed { get => isFormClosed; set => isFormClosed = value; }
        public frmLessons(Class CurrentClass, SchoolSubject SchoolSubject, bool ReadOnly)
        {
            InitializeComponent();

            currentClass = CurrentClass;
            currentLesson.IdClass = currentClass.IdClass;
            currentLesson.IdSchoolYear = currentClass.SchoolYear;
            currentLesson.IdSchoolSubject = SchoolSubject.IdSchoolSubject;
            currentSchoolSubject = SchoolSubject;
            txtSchoolSubject.Text = SchoolSubject.Name;

            string currentIdSchoolSubject = currentSchoolSubject.IdSchoolSubject;

            if (ReadOnly)
            {
                btnSaveTree.IsEnabled = false;
                btnAddNodeSon.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnLessonAdd.IsEnabled = false;
                btnLessonSave.IsEnabled = false;
                bntLessonErase.IsEnabled = false;
                btnAddNodeBrother.IsEnabled = false;
                this.Title += " (sola lettura)";
            }
            frmLessons_Load();
        }
        private void frmLessons_Load()
        {
            //txtLessonDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ////////dtpLessonDate.Value = new DateTime(1980, 01, 01);

            txtSchoolYear.Text = currentClass.SchoolYear;
            txtClass.Text = currentClass.Abbreviation;

            Lesson dummyLesson = Commons.bl.GetLastLesson(currentLesson);

            if (dummyLesson.IdSchoolSubject != null)
            {
                txtLessonCode.Text = dummyLesson.IdLesson.ToString();
                //////////dtpLessonDate.Value = (DateTime)dummyLesson.Date;
                TxtLessonDesc.Text = dummyLesson.Note;

                currentLesson.IdLesson = dummyLesson.IdLesson;
                currentLesson.Date = dummyLesson.Date;
                currentLesson.Note = dummyLesson.Note;
            }
            else
            {
                //dtpLessonDate.IsVisible = false; 
            }
            currentLessonsGridIndex = 0;
            // load data in datagrids
            RefreshLessons(currentLessonsGridIndex);

            topicTreeMptt = new TreeMptt(trwTopics, txtTopicName, txtTopicDescription,
                txtTopicSearchString, txtTopicsDigest, null, CommonsWpf.globalPicLed,
                chkSearchInDescriptions, chkVerbatimString, chkAllWord, chkCaseInsensitive,
                chkMarkAllTopicsFound, DragDropEffects.Copy, true);
            topicTreeMptt.AddNodesToTreeviewByBestMethod();

            RefreshTopicsChecksAndImages();

            ////////this.BackColor = CommonsWpf.ColorFromNumber(currentSchoolSubject);

            ////////LessonTimer.Interval = 1000;
            ////////LessonTimer.Start();

            isLoading = false;
        }
        private void RefreshLessons(int IndexInLessons)
        {
            List<SchoolGrades.BusinessObjects.Lesson> l = Commons.bl.GetLessonsOfClass(currentClass, currentLesson.IdSchoolSubject);
            dgwAllLessons.ItemsSource = l;
            if (l.Count > 0)
            {
                try
                {
                    ////////dgwAllLessons.ClearSelection();
                    ////////dgwAllLessons.Rows[IndexInLessons].Selected = true;
                }
                catch
                {

                }
            }
            else
            {
            }
            RefreshTopicsInOneLesson();
            RefreshTopicsChecksAndImages();
            if (dgwAllLessons.Columns.Count > 5)
            {
                ////////dgwAllLessons.Columns[0].IsVisible = false;
                ////////dgwAllLessons.Columns[1].IsVisible = true;
                ////////dgwAllLessons.Columns[2].IsVisible = false;
                ////////dgwAllLessons.Columns[3].IsVisible = false;
                ////////dgwAllLessons.Columns[4].IsVisible = true;
                ////////dgwAllLessons.Columns[5].IsVisible = false;
            }
        }
        private void RefreshTopicsInOneLesson()
        {
            dgwOneLesson.ItemsSource = Commons.bl.GetTopicsOfOneLessonOfClass(currentClass,
                    currentLesson);
            if (dgwOneLesson.Columns.Count > 5)
            {
                ////////dgwOneLesson.Columns[0].IsVisible = false;
                ////////dgwOneLesson.Columns[1].IsVisible = true;
                ////////dgwOneLesson.Columns[2].IsVisible = true;
                ////////dgwOneLesson.Columns[3].IsVisible = false;
                ////////dgwOneLesson.Columns[4].IsVisible = false;
                ////////dgwOneLesson.Columns[5].IsVisible = false;
                ////////dgwOneLesson.Columns[6].IsVisible = false;
            }
        }
        private void RefreshTopicsChecksAndImages()
        {
            //////////if (topicTreeMptt != null)
            //////////{
            //////////    topicTreeMptt.UncheckAllItemsUnderNode_Recursive((TreeViewItem)trwTopics.Items[0]);
            //////////    // gets and checks the topics of the current lesson 
            //////////    List<Topic> TopicsToCheck = Commons.bl.GetTopicsOfLesson(currentLesson.IdLesson);
            //////////    int dummy = 0;
            //////////    bool dummy2 = false;
            //////////    topicTreeMptt.CheckItemsInList_Recursive((TreeViewItem)trwTopics.Items[0],
            //////////    TopicsToCheck, ref dummy, ref dummy2);

            //////////    // gets the images associated with this lesson
            //////////    listImages = Commons.bl.GetListLessonsImages(currentLesson);
            //////////    // shows the fist image
            //////////    if (listImages != null && listImages.Count > 0)
            //////////        try
            //////////        {
            //////////            picImage.Load(Path.Combine(Commons.PathImages, listImages[indexImages].RelativePathAndFilename));
            //////////        }
            //////////        catch { }
            //////////    else
            //////////    {
            //////////        picImage.Image = null;
            //////////    }
            //////////}
        }
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            // ricerca 
            topicTreeMptt.FindNodes(txtTopicSearchString.Text);
        }
        private void btnAddNode_Click(object sender, RoutedEventArgs e)
        {
            topicTreeMptt.AddNewNode("Nuovo argomento", true);
            // set focus to the name textBox
            txtTopicName.Focus();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            topicTreeMptt.DeleteNodeSelected();
        }
        private void btnSaveTree_Click(object sender, RoutedEventArgs e)
        {
            if (!topicTreeMptt.HasChanges)
            {
                MessageBox.Show("Nessuna modifica fatta agli argomenti");
                return;
            }
            topicTreeMptt.SaveTreeFromTreeViewByParent();
            MessageBox.Show("Salvataggio fatto");
        }
        //private void ExportSubtreeToClipboard()
        //{
        //    TreeViewItem item = (TreeViewItem)(trwTopics.SelectedItem);
        //    if (item.Tag == null)
        //    {
        //        MessageBox.Show("Scegliere un argomento.\r\n" +
        //            "Verranno messi in clipboard gli argomenti dell'albero sotto l'argomento scelto");
        //        return;
        //    }
        //    string tree = null;
        //    Topic InitialNode = (Topic)item.Tag;

        //    topicTreeMptt.ExportSubtreeToText(InitialNode);

        //    Clipboard.SetText(tree);

        //    MessageBox.Show("Albero copiato nella clipboard");
        //}
        private void btnLessonAdd_Click(object sender, RoutedEventArgs e)
        {
            //////////dtpLessonDate.IsVisible = true;
            // if dtpLessonDate has its default date, no lesson is present in database 
            DateTime dummy = new DateTime(1980, 1, 1);
            ////////if (dtpLessonDate.Value.Date == dummy.Date)
            ////////{
            ////////    // first time, no lesson present, put current date in the control
            ////////    // the control will be used to know the date of the new lesson 
            ////////    dtpLessonDate.Value = DateTime.Now;
            ////////    currentLesson.Date = dtpLessonDate.Value;
            ////////    return;
            ////////}
            ////////else
            ////////{
            ////////    if (currentLesson.Date != dtpLessonDate.Value)
            ////////    {
            ////////        // date of current lesson and date in control dtpLessonDate are different
            ////////        // hence the user has changed the date to save a new lesson in a date different 
            ////////        // from today
            ////////        // ask for confirmation of saving in the new date
            ////////        if (MessageBox.Show("Creare una nuova lezione nella data del\n" +
            ////////            dtpLessonDate.Value.ToString("dd-MM-yyyy") + " (Sì)\n" +
            ////////            "Non salvare nulla (No)",
            ////////            "Creazione in data diversa")
            ////////            == MessageBoxResult.No)
            ////////        {
            ////////            return;
            ////////        }
            ////////    }
            ////////    else
            ////////    {
            ////////        // the date of the current lesson is the same displayed
            ////////        // then we'll create a new lesson for today 
            ////////        if (MessageBox.Show("Creare una nuova lezione nella data di oggi (Sì)" +
            ////////            "\nNon salvare nulla (No)",
            ////////            "Creazione in data odierna")
            ////////            == MessageBoxResult.No)
            ////////        {
            ////////            return;
            ////////        }
            ////////        else
            ////////        {
            ////////            // create the new lesson today 
            ////////            dtpLessonDate.Value = DateTime.Now;
            ////////        }
            ////////    }
            ////////}
            ////////Lesson l = Commons.bl.GetLessonInDate(currentClass, currentSchoolSubject.IdSchoolSubject,
            ////////    dtpLessonDate.Value);

            ////////if (l.IdLesson > 0)
            ////////{
            ////////    // found a lesson with the same date => block creation of the new lesson
            ////////    MessageBox.Show("Il programma non registra due lezioni diverse nello stesso giorno.\n" +
            ////////        "Nulla verrà salvato ora. Usare il bottone 'Salva'.",
            ////////        "ATTENZIONE");
            ////////    return;
            ////////}
            currentLesson = new Lesson();
            //////////currentLesson.Date = dtpLessonDate.Value;
            currentLesson.IdClass = currentClass.IdClass;
            currentLesson.IdSchoolSubject = currentSchoolSubject.IdSchoolSubject;
            currentLesson.IdSchoolYear = txtSchoolYear.Text;
            currentLesson.IdLesson = Commons.bl.NewLesson(currentLesson);
            TxtLessonDesc.Text = "";
            txtLessonCode.Text = currentLesson.IdLesson.ToString();
            //////////dtpLessonDate.Value = (DateTime)currentLesson.Date;
            topicTreeMptt.UncheckAllItemsUnderNode_Recursive((TreeViewItem)trwTopics.Items[0]);

            //  refresh database data in grids 
            RefreshLessons(0);
        }
        //private void txtLessonDesc_TextChanged(object sender, RoutedEventArgs e)
        //{

        //}
        private void btnLessonSave_Click(object sender, RoutedEventArgs e)
        {
            ////////int currentLessonsGridIndex = -1;
            ////////////////if (dgwAllLessons.CurrentRow != null)
            ////////////////{
            ////////////////    currentLessonsGridIndex = dgwAllLessons.CurrentRow.Index;
            ////////////////}
            ////////btnLessonSave.IsEnabled = false;
            ////////// save anyway (should be better to control if it is necessary)  
            ////////if (!topicTreeMptt.HasChanges)
            ////////{
            ////////    MessageBox.Show("Nessuna modifica fatta agli argomenti");
            ////////}
            ////////else
            ////////    topicTreeMptt.SaveTreeFromTreeViewByParent();

            ////////if (txtLessonCode.Text == "")
            ////////{
            ////////    MessageBox.Show("ATTENZIONE: Creare una nuova lezione!");
            ////////    btnLessonSave.IsEnabled = true;
            ////////    return;
            ////////}

            ////////if (dtpLessonDate.Value.Day != DateTime.Now.Day)
            ////////{
            ////////    if (MessageBox.Show("La data della lezione non è quella di oggi." +
            ////////        "\r\nVuoi salvarla comunque (Sì) o non salvarla (No)?",
            ////////        "")
            ////////        == MessageBoxResult.No)
            ////////    {
            ////////        btnLessonSave.IsEnabled = true;
            ////////        return;
            ////////    }
            ////////}
            ////////////////currentLesson.Date = dtpLessonDate.Value;
            ////////currentLesson.Note = TxtLessonDesc.Text;

            ////////// save the lesson (the topics could have been changed)
            ////////Commons.bl.SaveLesson(currentLesson);

            ////////// save the topics of the lesson
            ////////// we find the checked items in treeviw, we start from the beginning 
            ////////List<Topic> topicsOfTheLesson = new List<Topic>();
            ////////int dummy = 0;
            ////////topicTreeMptt.FindCheckedItems_Recursive((TreeViewItem)trwTopics.Items[0],
            ////////    topicsOfTheLesson, ref dummy);
            ////////if (topicsOfTheLesson.Count > 0)
            ////////    Commons.bl.SaveTopicsOfLesson(currentLesson.IdLesson, topicsOfTheLesson);

            //////////  refresh database data in grids 
            ////////if (currentLessonsGridIndex != -1)
            ////////    RefreshLessons(currentLessonsGridIndex);
            ////////RefreshTopicsChecksAndImages();

            ////////btnLessonSave.IsEnabled = true;
        }
        private void btnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TxtLessonDesc.Text + ". " + txtTopicsDigest.Text);
        }
        private void btnStartLinks_Click(object sender, RoutedEventArgs e)
        {
            List<StartLink> ll = Commons.bl.GetStartLinksOfClass(currentClass);
            foreach (StartLink link in ll)
            {
                try
                {
                    if (link.Link.Substring(0, 4) == "http")
                        Commons.ProcessStartLink(link.Link);
                    else
                        //Commons.ProcessStartLink(Commons.PathStartLinks + "\\" + link);
                        Commons.ProcessStartLink(Path.Combine(currentClass.PathRestrictedApplication, link.Link));
                }
                catch
                {
                    Console.Beep();
                }
            }
        }
        private void picImage_Click(object sender, RoutedEventArgs e)
        {

        }
        private void picImage_DoubleClick(object sender, RoutedEventArgs e)
        {
            if (listImages.Count > 0)
            {
                string fileName = Path.Combine(Commons.PathImages, listImages[indexImages].RelativePathAndFilename);
                if (File.Exists(fileName))
                {
                    Commons.ProcessStartLink(fileName);
                }
                else
                {
                    Console.Beep();
                    MessageBox.Show("Il file memorizzato dal database non esite più\n" + fileName);
                }
            }
        }
        private void btnManageImages_Click(object sender, RoutedEventArgs e)
        {
            if (txtLessonCode.Text == "")
            {
                MessageBox.Show("Creare prima una nuova lezione");
                return;
            }
            //////////frmImages fi = new frmImages(frmImages.ImagesFormType.NormalManagement
            //////////    , currentLesson, currentClass, listImages, currentSchoolSubject);
            //////////fi.ShowDialog();
            listImages = Commons.bl.GetListLessonsImages(currentLesson);
            if (listImages.Count > 0)
            {
                //////////indexImages = 0;
                //////////if (listImages.Count > 0)
                //////////{

                //////////    string nomeFile = Path.Combine(Commons.PathImages, listImages[indexImages].RelativePathAndFilename);
                //////////    try
                //////////    {
                //////////        picImage.Load(nomeFile);
                //////////    }
                //////////    catch
                //////////    {
                //////////        MessageBox.Show("Non è possible aprire il file " + nomeFile + " !");
                //////////    }
                //////////}
            }
            else
            {
                ////////picImage.Image = null;
            }
        }
        private void dgwOneLesson_CellContentClick(object sender, RoutedEventArgs e)
        {
            //////////if (e.RowIndex > -1)
            //////////{
            //////////    Topic row = ((List<Topic>)dgwOneLesson.ItemsSource)[e.RowIndex];
            //////////    topicTreeMptt.FindNodeById(row.Id);
            //////////}
        }
        private void frmLessonsTopics_KeyDown(object sender, KeyEventArgs e)
        {
            checkGeneralKeysForTopicsTree(e);
        }
        private void checkGeneralKeysForTopicsTree(KeyEventArgs e)
        {
            ////////if (e.KeyCode == Keys.F3)
            ////////    topicTreeMptt.FindNodes(txtTopicSearchString.Text, chkMarkAllNodesFound.IsChecked,
            ////////        true, false, false, false);
            ////////if (e.KeyCode == Keys.F5)
            ////////{
            ////////    btnSaveTree_Click(null, null);
            ////////}
        }
        private void checkSpecificKeysForTopicsTree(KeyEventArgs e)
        {
            ////////if (e.KeyCode == Keys.Right)
            ////////{
            ////////    NextImage();
            ////////}
            ////////if (e.KeyCode == Keys.Left)
            ////////{
            ////////    PreviousImage();
            ////////}

            ////////if (e.KeyCode == Keys.F3)
            ////////    topicTreeMptt.FindNodes(txtTopicSearchString.Text, chkMarkAllNodesFound.IsChecked,
            ////////        true, false, false, false);
            ////////if (e.KeyCode == Keys.F5)
            ////////{
            ////////    btnSaveTree_Click(null, null);
            ////////}
        }
        private void PreviousImage()
        {
            if (listImages.Count > 0)
            {
                if (indexImages == 0)
                    indexImages = listImages.Count;
                indexImages--;
                try
                {

                    ////////picImage.Load(Path.Combine(Commons.PathImages, listImages[indexImages].RelativePathAndFilename));
                }
                catch
                {
                    Console.Beep();
                }
            }
        }
        private void NextImage()
        {
            if (listImages.Count > 0)
            {
                indexImages = ++indexImages % listImages.Count;
                try
                {
                    //////////picImage.Load(Path.Combine(Commons.PathImages, listImages[indexImages].RelativePathAndFilename));
                }
                catch
                {
                    Console.Beep();
                }
            }
        }
        private void btnTopicsNotDone_Click(object sender, RoutedEventArgs e)
        {
            //////if (trwTopics.SelectedItem == null)
            //////{
            //////    MessageBox.Show("Scegliere un argomento.\r\n" +
            //////        "Verranno evidenziati gli argomenti sotto l'argomento scelto che NON sono stati fatti");
            //////    return;
            //////}
            //////List<Topic> listNonDone = Commons.bl.GetTopicsNotDoneFromThisTopic(currentClass,
            //////    ((Topic)trwTopics.SelectedItem.Tag), currentSchoolSubject);
            //////int dummy = 0; bool dummy2 = false;
            //////topicTreeMptt.HighlightNodesInList(trwTopics.Items[0],
            //////     listNonDone, ref dummy, ref dummy2);
        }
        private void btnTopicsDone_Click(object sender, RoutedEventArgs e)
        {
            ////////if (trwTopics.SelectedItem == null)
            ////////{
            ////////    MessageBox.Show("Scegliere un argomento con click.\r\n" +
            ////////        "Verranno evidenziati gli argomenti fatti sotto l'argomento scelto che sono stati fatti");
            ////////    return;
            ////////}
            ////////List<Topic> listDone = Commons.bl.GetTopicsDoneFromThisTopic(currentClass, ((Topic)trwTopics.SelectedItem.Tag), currentSchoolSubject);
            ////////int dummy = 0; bool dummy2 = false;
            ////////topicTreeMptt.HighlightNodesInList(trwTopics.Items[0],
            ////////     listDone, ref dummy, ref dummy2);
        }
        private void bntLessonErase_Click(object sender, RoutedEventArgs e)
        {
            ////////if (MessageBox.Show("Vuole davvero  eliminare la lezione:\r\n" + txtLessonCode.Text +
            ////////    ",'" + TxtLessonDesc.Text + "'?", "Cancellazione")
            ////////    != MessageBoxResult.Yes)
            ////////{
            ////////    return;
            ////////}
            ////////currentLessonsGridIndex = dgwAllLessons.CurrentRow.Index;
            ////////// !! TODO !! add message box to ask for image files deletion
            ////////Commons.bl.EraseLesson(int.Parse(txtLessonCode.Text), false);
            ////////RefreshLessons(currentLessonsGridIndex);
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            NextImage();
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            PreviousImage();
        }
        private void frmLessons_FormClosed(object sender, RoutedEventArgs e)
        {
            IsFormClosed = true;
        }
        private void btnArgFreemind_Click(object sender, RoutedEventArgs e)
        {
            //ExportSubtreeToClipboard();
            topicTreeMptt.ExportSubtreeToClipboard();
        }
        private void LessonTimer_Tick(object sender, RoutedEventArgs e)
        {
            ////////if (Application.OpenForms[0] != null)
            ////////{
            ////////    lblLessonTime.BackColor = ((frmMain)Application.OpenForms[0]).CurrentLessonTimeColor;
            ////////}
        }
        ////////private void DgwAllLessons_CellContentClick(object sender, RoutedEventArgs e)
        ////////{

        ////////}
        ////////private void DgwAllLessons_RowEnter(object sender, RoutedEventArgs e)
        ////////{
        ////////    if (e.RowIndex > -1)
        ////////    {
        ////////        dgwAllLessons.Rows[e.RowIndex].Selected = true;

        ////////        txtTopicsDigest.Text = "";
        ////////        List<Lesson> l = ((List<Lesson>)(dgwAllLessons.ItemsSource));

        ////////        if (currentLesson.IdLesson != l[e.RowIndex].IdLesson)
        ////////        {
        ////////            currentLesson.IdLesson = l[e.RowIndex].IdLesson;
        ////////            currentLesson.Note = l[e.RowIndex].Note;
        ////////            currentLesson.Date = l[e.RowIndex].Date;

        ////////            TxtLessonDesc.Text = currentLesson.Note;
        ////////            dtpLessonDate.Value = (DateTime)currentLesson.Date;
        ////////            txtLessonCode.Text = currentLesson.IdLesson.ToString();

        ////////            RefreshTopicsInOneLesson();
        ////////            RefreshTopicsChecksAndImages();
        ////////        }
        ////////    }
        ////////}
        ////////private void DgwAllLessons_CellClick(object sender, RoutedEventArgs e)
        ////////{
        ////////    if (e.RowIndex > -1)
        ////////    {
        ////////        dgwAllLessons.Rows[e.RowIndex].Selected = true;
        ////////    }
        ////////}
        ////////private void BtnSearchAmongTopics_Click(object sender, RoutedEventArgs e)
        ////////{
        ////////    int rowToBeSearchedIndex;

        ////////    if (dgwAllLessons.SelectedRows == null)
        ////////        rowToBeSearchedIndex = 0;
        ////////    else
        ////////        rowToBeSearchedIndex = dgwAllLessons.SelectedRows[0].Index;
        ////////    int indexWhenBeginning = rowToBeSearchedIndex;
        ////////    rowToBeSearchedIndex = ++rowToBeSearchedIndex % dgwAllLessons.Rows.Count;
        ////////    bool allScanned = false;
        ////////    while (!allScanned)
        ////////    {
        ////////        DataGridViewRow row = dgwAllLessons.Rows[rowToBeSearchedIndex];
        ////////        if (((string)row.Cells["Note"].Value).Contains(txtTopicsDigest.Text))
        ////////        {
        ////////            dgwAllLessons.ClearSelection();
        ////////            row.Selected = true;
        ////////            break;
        ////////        }
        ////////        rowToBeSearchedIndex = ++rowToBeSearchedIndex % dgwAllLessons.Rows.Count;
        ////////        if (rowToBeSearchedIndex == indexWhenBeginning)
        ////////            allScanned = true;
        ////////    }

        ////////}
        private void BtnOpenImagesFolder_Click(object sender, RoutedEventArgs e)
        {
            string folder = Path.Combine(Commons.PathImages,
                currentClass.SchoolYear + currentClass.Abbreviation,
                "Lessons", currentSchoolSubject.IdSchoolSubject);
            try
            {
                Commons.ProcessStartLink(folder);
            }
            catch
            {
                MessageBox.Show("La cartella non è stata ancora creata.\nIl programma la creerà automaticamente quando verrà salvata la prima immagine.");
            }
        }
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Da fare!");
        }
        private void btnAddNodeBrother_Click(object sender, RoutedEventArgs e)
        {
            topicTreeMptt.AddNewNode("Nuovo argomento", false);
            // set focus to the name textBox
            ////txtTopicName.Focus();
        }
        private void btnFindUnderNode_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Da fare!");
            //return; 
            topicTreeMptt.FindNodeUnderNode(txtTopicSearchString.Text);
        }
        private void chksSearch_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!isLoading)
            {
                // event fired when any of the checkboxes related to search is changed 

                // fire a new search 
                topicTreeMptt.ResetSearch();
                topicTreeMptt.FindNodes(txtTopicSearchString.Text);
            }
        }
    }
}

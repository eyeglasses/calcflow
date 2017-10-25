﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionMenu : MonoBehaviour {

    private CalcManager calcManager;
    private MultiSelectFlexPanel submissionsPanel;

    Matryx_Tournament tournament;
    private Dictionary<string, Matryx_Submission> submissions;


    private string[] Intro = { "A", "The" };
    private string[] Setup = { "Cure for our", "Cure for The", "Solution to The", "End of", "Quest for The", "Mission to", "Beginning of our", "Promise to Create The", "Beautiful Relationship with The" };
    private string[] adjectives = { "Untimely", "Fast-Approaching", "Promising", "Holy", "Perfect", "Incomprehensible", "Smart", "Advanced", "Crazy", "Hopeful", "Dreadful", "Rapidly Advancing", "Unstoppable", "Edible", "Uncontrollable", "Shameless", "Quantified", "Dangerous" };
    private string[] nouns = { "Zika virus", "Global Warming", "Educational Model", "Unified Quantum Theory", "Neural Implant", "Life-Harboring Planet", "Boredome", "Robot Overlords", "Star Destroyer", "Designer Baby", "Brain-Computer Interface", "Death", "Mars", "Sol", "Sexbot", "Nanobot", "Evolutionary Process", "Contact Lense", "Gene Therapy" };
    private string[] Outro = { ".", "We So Desperately Need.", "to Save Us All.", ": A Parent's Worst Nightmare.", "that Will End All Wars.", "to Revolutionize Just About Everything", ": A Journey", "that Might Kill Us All." };



    internal class KeyboardInputResponder : FlexMenu.FlexMenuResponder
    {
        SubmissionMenu submissionMenu;
        internal KeyboardInputResponder(SubmissionMenu submissionMenu)
        {
            this.submissionMenu = submissionMenu;
        }

        public void Flex_ActionStart(string name, FlexActionableComponent sender, GameObject collider)
        {
            //submissionMenu.HandleInput(sender.gameObject);
        }

        public void Flex_ActionEnd(string name, FlexActionableComponent sender, GameObject collider) { }
    }

    private Scroll scroll;
    const int maxTextLength = 400;

    JoyStickAggregator joyStickAggregator;
    FlexMenu flexMenu;

    public void Initialize(CalcManager calcManager)
    {
        this.calcManager = calcManager;

        scroll = GetComponentInChildren<Scroll>(true);
        flexMenu = GetComponent<FlexMenu>();
        KeyboardInputResponder responder = new KeyboardInputResponder(this);
        flexMenu.RegisterResponder(responder);
        submissionsPanel = GetComponentInChildren<MultiSelectFlexPanel>().Initialize();
        joyStickAggregator = scroll.GetComponent<JoyStickAggregator>();
    }

    public void SetTournament(Matryx_Tournament tournament)
    {
        if(this.tournament.descriptionAddress != tournament.descriptionAddress)
        {
            scroll.clear();
            LoadSubmissions(tournament);
            DisplaySubmissions();
            gameObject.SetActive(true);
        }
    }

    private void LoadSubmissions(Matryx_Tournament tournament)
    {
        submissions = new Dictionary<string, Matryx_Submission>();
        System.Random random = new System.Random();
        for (int i = 0; i < 15; i++)
        {
            int indexOne = random.Next(0, Intro.Length - 1);
            int indexTwo = random.Next(0, Setup.Length - 1);
            int indexThree = random.Next(0, adjectives.Length - 1);
            int indexFour = random.Next(0, this.nouns.Length - 1);
            int indexFive = random.Next(0, this.Outro.Length - 1);

            string intro = this.Intro[indexOne];
            string setup = this.Setup[indexTwo];
            string adjective = this.adjectives[indexThree];
            string noun = this.nouns[indexFour];
            string outro = this.Outro[indexFive];

            string description = intro + " " + setup + " " + adjective + " " + noun + " " + outro;

            Matryx_Submission aSubmission = new Matryx_Submission(description, random.Next(10000000, 200000000).GetHashCode().ToString());
            submissions.Add("" + i, aSubmission);
        }
    }

    public void DisplayTournamentDescription(string tournamentDescription)
    {
        //TODO: Write
    }

    public void DisplaySubmissions()
    {
        List<Transform> toAdd = new List<Transform>();
        foreach (Matryx_Submission submission in submissions.Values)
        {
            GameObject button = createButton(submission);
            button.SetActive(false);
            submissionsPanel.AddAction(button.GetComponent<FlexButtonComponent>());
        }
    }

    private GameObject createButton(Matryx_Submission submission)
    {
        GameObject button = Instantiate(Resources.Load("Submission_Cell", typeof(GameObject))) as GameObject;
        button.name = submission.name;
        button.GetComponent<SubmissionContainer>().SetSubmission(submission);

        string shortName = submission.name.Length > maxTextLength ? submission.name.Replace(submission.name.Substring(maxTextLength), "...") : submission.name;
        button.transform.Find("Text").GetComponent<TMPro.TextMeshPro>().text = shortName;

        scroll.addObject(button.transform);
        joyStickAggregator.AddForwarder(button.GetComponentInChildren<JoyStickForwarder>());

        return button;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class DialogueController : MonoBehaviour {
    // Two lists for each as I don't have time to discover tuples in C#
    List<int> OpeningNarrator;
    List<int> Mission1Narrator;
    List<int> Mission2Narrator;
    List<int> Mission3Narrator;
    List<int> EndingNarrator;

    List<string> Opening;
    List<string> Mission1;
    List<string> Mission2;
    List<string> Mission3;
    List<string> Ending;

    public bool DialogueGoing = true;
    public int DialogueNumber = 0;
    public List<int> mCurrentNarrator;
    public List<string> mCurrent;

    public GameObject mChildText;
    public GameObject mDialogueBox;
    public GameObject mCharacterPicker;

    public GameObject Speaker;
    public List<Transform> SpeakerChildren;

    void Awake()
    {
        DialogueGoing = true;
        OpeningNarrator = new List<int>();
        Mission1Narrator = new List<int>();
        Mission2Narrator = new List<int>();
        Mission3Narrator = new List<int>();
        EndingNarrator = new List<int>();

        Opening = new List<string>();
        Mission1 = new List<string>();
        Mission2 = new List<string>();
        Mission3 = new List<string>();
        Ending = new List<string>();

        Speaker = GameObject.Find("Speaker");
        SpeakerChildren = new List<Transform>();
        if (Speaker != null)
        {
            foreach (Transform child in GameObject.Find("Speaker").transform)
            {
                SpeakerChildren.Add(child);
                child.gameObject.SetActive(false);
            }
            Speaker.SetActive(false);
        }


        populateLists();

        switch (SceneManager.GetActiveScene().name)
        {
            case ("Opening"):
                mCurrentNarrator = OpeningNarrator;
                mCurrent = Opening;
                StartCoroutine(RunAutoDialogue());
                break;
            case ("Mission_1"):
                mCurrentNarrator = Mission1Narrator;
                mCurrent = Mission1;
                break;
            case ("Mission_2"):
                mCurrentNarrator = Mission2Narrator;
                mCurrent = Mission2;
                break;
            case ("Mission_3"):
                mCurrentNarrator = Mission3Narrator;
                mCurrent = Mission3;
                break;
            case ("Ending"):
                mCurrentNarrator = EndingNarrator;
                mCurrent = Ending;
                StartCoroutine(RunAutoDialogue());
                break;
            default:
                Debug.Log("YO");
                break;
        }
        mChildText.GetComponent<Text>().text = mCurrent[DialogueNumber++];
    }

    // 1 is no narrator
    // 2 is Aurora narrator
    // 3 is character 1 narrator
    // 4 is informant narrator
    // 5 is question mark narrator
    // 99 Marks the middle dialogue

    public void populateLists()
    {
        OpeningNarrator.Add(1);
        Opening.Add("By the year 2117, rising sea levels and temperature shifts have rendered much of the world uninhabitable. The population has been forced inland into massive megacities and arcologies.");
        OpeningNarrator.Add(1);
        Opening.Add("Several key corporations used the chaos of the 21st century to gradually push their control over the populace and the government. By the present, these corporations are essentially tyrants, promoting the lives of the the wealthy and powerful while grinding the majority of people to dust.");
        OpeningNarrator.Add(1);
        Opening.Add("You are Aurora Price, hacker and revolutionary. You have been fighting the corporations with your team of outlaws and weirdos. So far you have remained out from under their thumb, but every job pushes just a little bit farther...");

        Mission1Narrator.Add(1);
        Mission1.Add("MISSION 1");
        Mission1Narrator.Add(2);
        Mission1.Add("Alright guys, hitting another Nile data facility. This one’s pretty standard, all ya gotta do is take out the guards, get to the server room, and plug in the comm unit. Good luck team!");
        Mission1Narrator.Add(3);
        Mission1.Add("Okay boss, talk to ya in a couple minutes.");
        Mission1Narrator.Add(99);
        Mission1.Add("");
        Mission1Narrator.Add(2);
        Mission1.Add("Another job well done. We got tons of good data for transfer…");
        Mission1.Add("Wait, what’s this? Confidential shipments from Ares Technologies?");
        Mission1Narrator.Add(3);
        Mission1.Add("Nile? But they’re just a delivery company. What are they getting from a weapons manufacturer?");
        Mission1Narrator.Add(2);
        Mission1.Add("That’s a good question, I’ll set up a meeting with one of my sources from Nile.");

        Mission2Narrator.Add(1);
        Mission2.Add("MISSION 2");
        Mission2Narrator.Add(2);
        Mission2.Add("Alright, we’re gonna be meeting up with an informant from one of the Nile Deliveries research team, Phoenix. He asked us to meet him in that alley over there, so head over with the comm unit. Keep an eye out!");
        Mission2Narrator.Add(4);
        Mission2.Add("H-h-hey guys, are you Aurora’s entourage or something?");
        Mission2Narrator.Add(3);
        Mission2.Add("Yeah, you Phoenix? The boss said to tell you “the salmon run was late this year, whatever that means");
        Mission2Narrator.Add(4);
        Mission2.Add("You have no idea how happy I am to hear that! I could get killed if anyone from work caught me out here.");
        Mission2Narrator.Add(3);
        Mission2.Add("Well lets try to keep that from … Wait, what was that?");
        Mission2Narrator.Add(3);
        Mission2.Add("Stay back, Phoenix! Looks like this is gonna get messy.");
        Mission2Narrator.Add(99);
        Mission2.Add("");
        Mission2Narrator.Add(2);
        Mission2.Add("That was some real firepower! You had better tell us what this is all about, Phoenix.");
        Mission2Narrator.Add(4);
        Mission2.Add("Alright, I looked into what Aurora asked me to, and it does not look good. It turns out that Nile commissioned some pretty serious stuff from Ares.");
        Mission2Narrator.Add(4);
        Mission2.Add("They’re calling it “Parerinol” and it seems like a chemical that increases suggestibility and consumption.");
        Mission2Narrator.Add(4);
        Mission2.Add("They took it to a research facility underneath their headquarters, I think that they’re going to put it in the cities water supply!");
        Mission2Narrator.Add(4);
        Mission2.Add("Here’s the technical specifications of the lab for Aurora. I can get you to the lab, but i won't be able to deal with the guards.");
        Mission2Narrator.Add(3);
        Mission2.Add("Well thanks Phoenix, you’ve done a good thing in speaking out here. We’ll do everything we can to take care of this without exposing you.");
        Mission2Narrator.Add(3);
        Mission2.Add("Aurora, we’re heading back to base.");

        Mission3Narrator.Add(1);
        Mission3.Add("MISSION 3");
        Mission3Narrator.Add(2);
        Mission3.Add("Team, this is a big mission, we’ll be going into a secret lab underneath Nile Deliveries HQ, where they are going to put a mind-suppressing drug in the water supply. We have to destroy it!");
        Mission3Narrator.Add(2);
        Mission3.Add("You have the map of the lab as supplied by Phoenix, you’ll want to take out all the guards so you can plant the explosives.");
        Mission3Narrator.Add(99);
        Mission3.Add("");
        Mission3Narrator.Add(2);
        Mission3.Add("Great job taking out the guards guys, now just get the explosives planted and get the hell out of there.");
        Mission3Narrator.Add(2);
        Mission3.Add("After this we should all try to keep a low profile for a while. It was amazing working with you all, and I hope the best for all of you.");

        EndingNarrator.Add(1);
        Ending.Add("Several weeks later...");
        EndingNarrator.Add(1);
        Ending.Add("*Door knocking*");
        EndingNarrator.Add(2);
        Ending.Add("Who's there?");
        EndingNarrator.Add(1);
        Ending.Add("...");
        EndingNarrator.Add(2);
        Ending.Add("Wait... who are you guys?");
        EndingNarrator.Add(5);
        Ending.Add("Miss Price, you're going to have to come with us.");
    }

    public void WonMission()
    {
        DialogueGoing = true;
        mDialogueBox.SetActive(true);
        NextDialogue();
    }

    public void NextDialogue()
    {
        foreach (Transform child in SpeakerChildren)
        {
            if (child.gameObject.activeInHierarchy)
                child.gameObject.SetActive(false);
        }

        switch (mCurrentNarrator[DialogueNumber] - 2)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                Speaker.SetActive(true);
                SpeakerChildren[mCurrentNarrator[DialogueNumber] - 2].gameObject.SetActive(true);
                break;
            default:
                Speaker.SetActive(false);
                break;
        }
        if (mCurrentNarrator[DialogueNumber] - 2 < 5 && mCurrentNarrator[DialogueNumber] - 2 > 0)
            SpeakerChildren[mCurrentNarrator[DialogueNumber] - 2].gameObject.SetActive(true);
        mChildText.GetComponent<Text>().text = mCurrent[DialogueNumber];
        if (mCurrentNarrator[DialogueNumber] == 99)
        {
            mCharacterPicker.SetActive(true);
            mCharacterPicker.GetComponent<CharacterPicker>(). SelectingCharacters = true;
            mDialogueBox.SetActive(false);
        }
        DialogueNumber++;

        if (DialogueNumber == mCurrentNarrator.Count)
        {
            if (SceneManager.GetActiveScene().buildIndex == 4)
                Application.Quit();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


    void Update ()
    {
        if (mCharacterPicker == null)
            return;
        if (!mCharacterPicker.activeInHierarchy && mCharacterPicker.GetComponent<CharacterPicker>().SelectingCharacters == true)
        {
            mCharacterPicker.GetComponent<CharacterPicker>().SelectingCharacters = false;
            DialogueGoing = false;
        }
    }

    IEnumerator RunAutoDialogue()
    {
        foreach(string myText in mCurrent)
        {
            mChildText.GetComponent<Text>().text = myText;
            yield return new WaitForSeconds(15);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

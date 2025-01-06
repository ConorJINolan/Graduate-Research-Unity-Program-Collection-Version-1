using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class script : MonoBehaviour
{
    // HapticController variables
    private HapticMaterial hapticMaterial;
    public GameObject hapticFieldPrefab;
    public string ForceDisplayText = "";
    public Text ForceDisplay;
    private int forceCount = 0;
    private List<Force> forceList;
    public Force lowViscosity = new Force(0.25f, 0.25f, 0.25f, 5.0f, 0.2f, 0.0f, 0.0f, Vector3.zero, 0.0f, Vector3.zero, 0.0f, 0.5f);
    public Force highViscosity = new Force(1.0f, 1.0f, 1.0f, 20.0f, 0.95f, 0.0f, 0.0f, Vector3.zero, 0.0f, Vector3.zero, 0.0f, 0.5f);
    
    public Force resetForce = new Force(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, Vector3.zero, 0.0f, Vector3.zero, 0.0f, 0.5f);

    public int secondSet = 0; 
    // Remaining Systems
    public Transform HapticCollider;
    public GameObject SpawnSphere;
    public Transform SpawnedSphere;
    public Text XText, YText, ZText, AText, MText, Itr, NamedDisplay, IsRecording;
    public GameObject small, big;
    public Text timerText;

    public float rawtxtCheck = 0;
    private string csvDocumentName;
    private string rawCsvDocumentName;
    public string DATE = "B";
    private string FILENAME = "";
    private string FOLDERNAME = "";
    private string ITERATION = "1st";
    private bool isRecording = false;

    public float XValue, YValue, ZValue, TValue;
    public float AChange = 0;
    public float angleChange = -1;
    public float Iter = 1;
    public float timeValue = 0;
    public float Spc = 0;

    public float repsPerSet = 4;
    private int currentRep = 1;
    private int currentSet = 1;
    private bool isProgramRunning = false;

    public Text instructionText;
    public Text countText;
    public GameObject instructionScreen;
    public GameObject nameScreen;

    private string txtDocumentName;
    private string rawtxtDocumentName;

    // User Inputs
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public Button submitButton;
    public string fullName = "";
    private bool isTimerRunning = false;
    public float size = 1;
    float state = 0;
    float crcl = 0;
    int repPart = 1;
    float xCheck = 0;
    float yCheck = 0;
    float zCheck = 0;
    public float txtCheck = 0;
    public float txtUpdateSrt = 0; 
    public float txtUpdateEnd = 0;
    public float rawtxtUpdateSrt = 0; 
    public float rawtxtUpdateEnd = 0;
    public Vector3 tempPosition;
    public Vector3 activePosition;
    // Buffer Values
    private float spawnInterval = 0.1f; // Spawn every 0.5 seconds
    private float spawnTimer = 0f;
    private List<string> dataBuffer = new List<string>();
    private float fileWriteInterval = 5f; // Write to file every 5 seconds
    private float fileWriteTimer = 0f;
    void Start()
    {
        ResetProgram();
        // Initialize UI and file listeners
        submitButton.onClick.AddListener(OnSubmit);
        Vector3 tempPosition = small.transform.position;
        Vector3 activePosition = big.transform.position;
        ShowNameScreen();
        UpdateCountDisplay();

        // Initialize haptic material and force list
        hapticMaterial = hapticFieldPrefab.GetComponent<HapticMaterial>();
        forceList = new List<Force> { lowViscosity, highViscosity };
    }

    void Update()
    {   
        float XValue = transform.position.x;
        float YValue = transform.position.y;
        float ZValue = transform.position.z;
        if (!isProgramRunning)
        {
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
            {
                HideInstructionScreen();
                isProgramRunning = true;
            }
        }
        else
        {
            // When Space is pressed, toggle recording and start the timer
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleRecording();
                // Start the timer after the first Space bar press
                if (!isTimerRunning)
                {
                    isTimerRunning = true;  // Start the timer
                    timeValue = 0; // Reset time to 0
                }
            }

            if (isTimerRunning)  // Only update the timer if it's running
            {
                timeValue += Time.deltaTime;
                DisplayTime(timeValue);
            }
                if (isRecording)
            {
                    spawnTimer += Time.deltaTime;
                    CreateRawTextFile();
                    if (spawnTimer >= spawnInterval)
                    {
                        spawnTimer = 0f;
                        // Instantiate the object each frame while recording is on
                        GameObject clone = Instantiate(SpawnSphere, new Vector3(transform.position.x, transform.position.y, 90), transform.rotation);
                        clone.transform.SetParent(SpawnedSphere);
                        dataBuffer.Add($"{ForceDisplay.text}\t{currentRep}\t{currentSet}\t{timerText.text}\t{XText.text}\t{YText.text}\t{ZText.text}\t{AText.text}\t{MText.text}");

                        fileWriteTimer += Time.deltaTime;
                        if (fileWriteTimer >= fileWriteInterval)
                        {
                            fileWriteTimer = 0f;
                            WriteBufferedDataToFile();
                        
                        }
                    }
            }
  


            DisplayX(XValue);
            DisplayY(YValue);
            DisplayZ(ZValue);

            MagnitudesAngles(XValue, YValue);
        }
    }
private void WriteBufferedDataToFile()
{
    if (dataBuffer.Count > 0)
    {
        File.AppendAllLines(txtDocumentName, dataBuffer);
        dataBuffer.Clear();
    }
}
void DisplayX(float XToDisplay)

    {
        xCheck += 1;

        float Xtransition = XToDisplay;

        if (size == 1)
        {

            state = 1;
            
        }
        else if (size == 2)
        {
            if (crcl == 1)
            {
                state = 2;
                Xtransition += 5;
            }
            else if (crcl == 2)
            {
                state = 3;
                Xtransition += 0;
            }
            else if (crcl == 3)
            {
                state = 4;
                Xtransition += -5;
            }
        }

                XText.text = string.Format("{0:00.0000}", Xtransition);

    }

void DisplayY(float YToDisplay)

    {

        yCheck += 1;

        float Ytransition = YToDisplay;


        if (Iter == 1)
        {
            ITERATION = "1st";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 3, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 3, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 3, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 3, 332);
                }
            }
        }
        else if (Iter == 2)
        {
            ITERATION = "2nd";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 103, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 103, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 103, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 103, 332);
                }
            }
        }
        else if (Iter == 3)
        {
            ITERATION = "3rd";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 203, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 203, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 203, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 203, 332);
                }
            }
        }
        else if (Iter == 4)
        {
            ITERATION = "4th";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 303, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 303, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 303, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 303, 332);
                }
            }
        }
        else if (Iter == 5)
        {
            ITERATION = "5th";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 403, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 403, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 403, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 403, 332);
                }
            }
        }
        else if (Iter == 6)
        {
            ITERATION = "6th";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 503, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 503, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 503, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 503, 332);
                }
            }
        }
        else if (Iter == 7)
        {
            ITERATION = "7th";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 603, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 603, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 6203, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 603, 332);
                }
            }
        }
        else if (Iter == 8)
        {
            ITERATION = "8th";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 703, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 703, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 703, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 703, 332);
                }
            }
        }
        else if (Iter == 9)
        {
            ITERATION = "9th";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 803, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 803, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 803, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 803, 332);
                }
            }
        }
        else if (Iter == 10)
        {
            ITERATION = "10th";
            if (size == 1)
            {

                SpawnedSphere.transform.position = new Vector3(7, 903, 32);

            }
            else if (size == 2)
            {
                if (crcl == 1)
                {
                    SpawnedSphere.transform.position = new Vector3(107, 903, 132);
                }
                else if (crcl == 2)
                {
                    SpawnedSphere.transform.position = new Vector3(207, 903, 232);
                }
                else if (crcl == 3)
                {
                    SpawnedSphere.transform.position = new Vector3(307, 903, 332);
                }
            }
        }
        else if (Iter == 10)
        {
            ITERATION = "Done";
        }


            YText.text = string.Format("{0:00.0000}", Ytransition);

    }

void DisplayZ(float ZToDisplay)

    {
        /*
        if (ZToDisplay < 0)

        {

            ZToDisplay = 0;

        }
        */

        zCheck += 1;
        float Ztransition = ZToDisplay;

        if (size == 1)
        {

           
        }
        else if (size == 2)
        {
            if (crcl == 1)
            {
              
            }
            else if (crcl == 2)
            {
              
            }
            else if (crcl == 3)
            {
                
            }
        }

        ZText.text = string.Format("{0:00.0000}", Ztransition);

    }

    //timer function
    void DisplayTime(float timeToDisplay)

    {

        if (timeToDisplay < 0)

        {

            timeToDisplay = 0;

        }



        float minutes = Mathf.FloorToInt(timeToDisplay / 60);

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        float milliseconds = timeToDisplay % 1 * 1000;



        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);



    }
void MagnitudesAngles(float relativex,float relativey)
    {
        //Vector3 relative = transform.InverseTransformPoint(transform.position);
        float relx = relativex;

        if (size == 1)
        {

            relx = relativex;

        }
        else if (size == 2)
        {
            if (crcl == 1)
            {
                relx = relativex +5;
            }
            else if (crcl == 2)
            {
                relx = relativex;
            }
            else if (crcl == 3)
            {
                relx = relativex - 5;
            }
        }

        float MagCurrent = Mathf.Sqrt(Mathf.Pow(relx, 2) + Mathf.Pow(relativey, 2));

        float AngleCurrent = Mathf.Atan2(relativey, relx) * Mathf.Rad2Deg;
        // if (size == 1)
        //{
        //     float MagCurrent = Mathf.Sqrt(Mathf.Pow(relativex, 2) + Mathf.Pow(relativey, 2));
        // }
        // else if (size == 2)
        // {
        //  float MagCurrent = Mathf.Sqrt(Mathf.Pow(relativex, 2) + Mathf.Pow(relativey, 2));
        // }

       AText.text = string.Format("{0:000.000}", AngleCurrent);
       MText.text = string.Format("{0:000.000}", MagCurrent);
       string Astring = string.Format("{0:000}", AngleCurrent);
        


       AChange = float.Parse(Astring);
    }

    void OnSubmit()
    {
        string firstName = firstNameInput.text.Trim();
        string lastName = lastNameInput.text.Trim();

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
        {
            Debug.LogWarning("Please fill in both First Name and Last Name fields.");
            ShowNameScreen();
            return;
        }

        fullName = $"{firstName} {lastName}";
        DATE = System.DateTime.Now.ToString("yyyy-MM-dd");

        // Generate file and folder names
        FILENAME = $"{fullName.Replace(" ", "_")}_{DATE}";
        FOLDERNAME = $"{fullName.Replace(" ", "-")}-{DATE}";
        Debug.Log($"Generated FILENAME: {FILENAME}, FOLDERNAME: {FOLDERNAME}");

        // Create files and directories
        CreateTextFile();  // Ensure files are created after name is entered

        // Update UI
        NamedDisplay.text = FILENAME;

        // Hide name screen and start the program
        HideNameScreen();
        
        ShowInstructionScreenVoid();
    }

public void CreateTextFile()
{
    txtUpdateSrt += 1;
    string FILENAME = fullName + "_" + DATE;
    string FOLDERNAME = fullName + "-" + DATE;

    // Ensure the folder exists
    string folderPath = Path.Combine(Application.streamingAssetsPath, FOLDERNAME);
    if (!Directory.Exists(folderPath))
    {
        Directory.CreateDirectory(folderPath); // Create folder if it doesn't exist
    }

    if (size == 1)
    {
        string txtDocumentName = Path.Combine(folderPath, ITERATION + "_" + FILENAME + "_big0.txt");

        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, ITERATION + "_" + FILENAME + "\n");
            // Add header for the new variables
            File.AppendAllText(txtDocumentName, "ForceDisplay" + "\t" + "Rep" + "\t" + "Set" + "\t" + "timer" + "\t" + "X" + "\t" + "Y" + "\t" + "Z" + "\t" + "A" + "\t" + "M" + "\n");
            txtCheck += 1;
        }

        // Append real-time data including ForceDisplay, currentRep, and currentSet
        File.AppendAllText(txtDocumentName, ForceDisplay.text + "\t" + currentRep + "\t" + currentSet + "\t" + timerText.text + "\t" + XText.text + "\t" + YText.text + "\t" + ZText.text + "\t" + AText.text + "\t" + MText.text);
        File.AppendAllText(txtDocumentName, "\n");
    }

    if (size == 2)
    {
        string suffix = crcl switch
        {
            1 => "_small1.txt",
            2 => "_small2.txt",
            3 => "_small3.txt",
            _ => "_unknown.txt"
        };

        string txtDocumentName = Path.Combine(folderPath, ITERATION + "_" + FILENAME + suffix);

        if (!File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, ITERATION + "_" + FILENAME + "\n");
            // Add header for the new variables
            File.AppendAllText(txtDocumentName, "ForceDisplay" + "\t" + "Rep" + "\t" + "Set" + "\t" + "timer" + "\t" + "X" + "\t" + "Y" + "\t" + "Z" + "\t" + "A" + "\t" + "M" + "\n");
            txtCheck += 1;
        }

        // Append real-time data including ForceDisplay, currentRep, and currentSet
        File.AppendAllText(txtDocumentName, ForceDisplay.text + "\t" + currentRep + "\t" + currentSet + "\t" + timerText.text + "\t" + XText.text + "\t" + YText.text + "\t" + ZText.text + "\t" + AText.text + "\t" + MText.text);
        File.AppendAllText(txtDocumentName, "\n");
    }

    txtUpdateEnd += 1;
}

   

    public void ChangeForce(Force force)
    {
        hapticMaterial.hMass = force.mass;
        hapticMaterial.hStiffness = force.stiffness;
        hapticMaterial.hDamping = force.damping;
        hapticMaterial.hImpulseD = force.impulseDepth;
        hapticMaterial.hViscosity = force.viscosity;
        hapticMaterial.hFrictionS = force.staticFriction;
        hapticMaterial.hFrictionD = force.dynamicFriction;
        hapticMaterial.hConstForceDir = force.constantForceDir;
        hapticMaterial.hConstForceMag = force.constantForceMag;
        hapticMaterial.hSpringDir = force.springDir;
        hapticMaterial.hSpringMag = force.springMagnitude;
        hapticMaterial.hPopthAbs = force.popThroughForce;
    }

    // Method to update the UI text displaying the current force type
    void DisplayForce(string forceName)
    {
        ForceDisplay.text = forceName;
    }

    private void ShowInstructionScreen(string message)
    {
        instructionScreen.SetActive(true);
        instructionText.text = message;
    }
    public void ShowInstructionScreenVoid()
    {
        instructionScreen.SetActive(true);
    }

    private void ShowNameScreen()
    {
        nameScreen.SetActive(true);
    }

    private void HideNameScreen()
    {
        nameScreen.SetActive(false);
    }

    private void HideInstructionScreen()
    {
        instructionScreen.SetActive(false);
    }


private void ToggleRecording()
{
    isRecording = !isRecording;

    if (isRecording)
    {
        Spc = 1;
        if (IsRecording != null)
        {
            IsRecording.color = Color.green;
            IsRecording.text = "Recording";
        }
        HideInstructionScreen();

        ChangeForce(forceList[forceCount]);
        DisplayForce(forceCount == 0 ? "Low Viscosity" : "High Viscosity");
        forceCount = (forceCount + 1) % forceList.Count;
        angleChange = AChange;
    }
    else
    {
        // When stopping recording
        Spc = 2;
        if (IsRecording != null)
        {
            IsRecording.color = Color.red;
            IsRecording.text = "Not Recording";
        }
        ClearSpawnedSpheres();

        RecordRep();

        // Set the target position to (0, 0, 100)
        Vector3 targetPosition = new Vector3(0, 0, 100);

        // Switch the visibility and positions based on reps
        if(repPart > 1) // For reps 2, 3, 4, 6, 7, 8, etc., small is active
        {
            big.SetActive(false);  // Hide big
            small.SetActive(true);  // Show small
            big.transform.position = targetPosition;  // Set big position to (0, 0, 100)
            small.transform.position = targetPosition; // Set small position to (0, 0, 100)
        }
        else
        {

            big.SetActive(true);   // Show big
            small.SetActive(false); // Hide small
            big.transform.position = targetPosition;  // Set big position to (0, 0, 100)
            small.transform.position = targetPosition; // Set small position to (0, 0, 100)


        }
    }
}



public void CreateRawTextFile()
{
    string FILENAME = fullName + "_" + DATE;
    string FOLDERNAME = fullName + "-" + DATE;
    rawtxtUpdateSrt += 1;

    // Ensure the folder exists
    string folderPath = Path.Combine(Application.streamingAssetsPath, FOLDERNAME);
    if (!Directory.Exists(folderPath))
    {
        Directory.CreateDirectory(folderPath); // Create folder if it doesn't exist
    }

    string rawtxtDocumentName = Path.Combine(folderPath, ITERATION + "_" + FILENAME + "_raw0.txt");

    if (!File.Exists(rawtxtDocumentName))
    {
        File.WriteAllText(rawtxtDocumentName, ITERATION + "_" + FILENAME + "_raw\n \n");
        // Add header for the new variables
        File.AppendAllText(rawtxtDocumentName, "ForceDisplay" + "\t" + "Rep" + "\t" + "Set" + "\t" + "timer" + "\t" + "X" + "\t" + "Y" + "\t" + "Z" + "\t" + "A" + "\t" + "M" + "\n");
        rawtxtCheck += 1;
    }

    // Append real-time data including ForceDisplay, currentRep, and currentSet
    File.AppendAllText(rawtxtDocumentName, ForceDisplay.text + "\t" + currentRep + "\t" + currentSet + "\t" + timerText.text + "\t" + XText.text + "\t" + YText.text + "\t" + ZText.text + "\t" + AText.text + "\t" + MText.text);
    File.AppendAllText(rawtxtDocumentName, "\n");

    rawtxtUpdateEnd += 1;
}

private void RecordRep()
    {
       
        if (currentRep >= repsPerSet)
        {
            currentRep = 1;
            currentSet++;
            if (currentSet > 2)
            {   
                repPart = 1;
                ShowInstructionScreen("Program completed! Press any key to restart.");
                ResetProgram();
                return;
            }
        }
        repPart++;
        if(repPart == 5)
        {
            currentRep++;
            repPart = 1;
        }
        UpdateCountDisplay();
        ShowInstructionScreen($"Set {currentSet} | Rep {currentRep} of {repsPerSet}\nPress Space to continue.");
    }

    private void UpdateCountDisplay()
    {
        countText.text = $"Set {currentSet} | Rep {currentRep} of {repsPerSet}";
        Itr.text = $"Iteration: {ITERATION}";
    }

    private void ResetProgram()
    {
        // Reset variables to initial states
        Vector3 targetPosition = new Vector3(0, 0, 100);
        isProgramRunning = false;
        isRecording = false;
        currentRep = 0;  // Start from first rep
        currentSet = 1;  // Start from first set
        Iter = 1;        // Reset iteration
        Spc = 0;         // Reset Space press state
        timeValue = 0;   // Reset time value
        isTimerRunning = false;  // Ensure timer stops until the first Space press
        secondSet = 0;
        big.SetActive(true);   // Show big
        small.SetActive(false); // Hide small
        big.transform.position = targetPosition;  // Set big position to (0, 0, 100)
        small.transform.position = targetPosition; // Set small position to (0, 0, 100)

        // Clear any previously generated data
        ClearSpawnedSpheres();

        // Reset all UI text
        XText.text = "0.0000";
        YText.text = "0.0000";
        ZText.text = "0.0000";
        AText.text = "0.000";
        MText.text = "0.000";
        ForceDisplay.text = "None";
        timerText.text = "00:00:000"; // Reset timer display

        // Hide instruction screen if visible
        HideInstructionScreen();

        // Show name input screen again
        ShowNameScreen();

        // Reset file names
        fullName = "";  // Reset name
        FILENAME = "";  // Reset filename
        FOLDERNAME = ""; // Reset folder name

        // Optionally reset any input field values
        firstNameInput.text = "";
        lastNameInput.text = "";

        // Update the UI for name input screen
       NamedDisplay.text = ""; 

        // Display instructions for the user
        DisplayForce("None");
        UpdateCountDisplay();
    }

    private void ClearSpawnedSpheres()
    {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.StartsWith(SpawnSphere.name) && obj.name.Contains("(Clone)"))
                {
                    Destroy(obj);
                }
            }
    }
}

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start");
        Console.WriteLine("");

        // create each job object
        job job1 = new job("Microsoft", "Software Engineer", 2019, 2022);
        job job2 = new job("Apple", "Manager", 2022, 2023);

        // create a list to hold the jobs
        List<job> listOfJobs = new List<job>();

        // add the jobs to the list
        listOfJobs.Add(job1);
        listOfJobs.Add(job2);

        // create a resume object
        resume resume1 = new resume("Allison Rose", listOfJobs);


        // display the resume
        Console.WriteLine(resume1.display());

        Console.WriteLine($"\nEND");
    }
}

public class job {

    // declare out values
    private string company = "";
    private string jobTitle = "";
    private int startYear = 0;
    private int endYear = 0;

    public job(){

    }

    public job(string companyValue, string jobTitleValue, int startYearValue, int endYearValue){
        company = companyValue;
        jobTitle = jobTitleValue;
        startYear = startYearValue;
        endYear = endYearValue;
    }

    public string display(){
        return ($"{jobTitle} ({company}) {startYear}-{endYear}");
    }

    // getters
    public string getCompany(){
        return company;
    }

    public string getJobTitle(){
        return jobTitle;
    }

    public int getStartYear(){
        return startYear;
    }

    public int getEndYear(){
        return endYear;
    }

    

    // setters
    public void setCompany(string companyValue){
        company = companyValue;
    }

    public void setJobTitle(string jobTitleValue){
        jobTitle = jobTitleValue;
    }

    public void setStartYear(int startYearValue){
        startYear = startYearValue;
    }

    public void setEndYear(int endYearValue){
        endYear = endYearValue;
    }



    
}

public class resume{
    private List<job> jobs = new List<job>();

    private string name = "";

    public resume(){

    }

    public resume(string nameValue, List<job> jobsList){
        name = nameValue;
        jobs = jobsList;
    }

    //setters

    public void setName(string nameValue){
        name = nameValue;
    }

    public void setJobs(List<job> jobsList){
        jobs = jobsList;
    }

    public void appendJobs(job jobValue){
        jobs.Add(jobValue);
    }

    //getters
    public string getName(){
        return name;
    }

    public List<job> getJobs(){
        return jobs;
    }

    public int inJobs(job value){
        return jobs.IndexOf(value);
    }

    public job jobsIndex(int index){
        return jobs[index];
    }


    public string display(){
        string output = name;

        for (int i = 0; i < jobs.Count; i++){
            output = output + ($"\n   {jobsIndex(i).display()}");
        }

        return output;
    }

}
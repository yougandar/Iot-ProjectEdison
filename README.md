## ARM Template Deployment Using Azure Portal

1.	Click the below **Git hub** repo URL.

https://github.com/sysgain/Iot-ProjectEdison/tree/stage

2.	Select **main-template.json** from **stage** branch, click on **Raw** from the top right corner. **Copy** the raw template and **paste** in your Azure portal for template deployment.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd1.png)

#### To deploy a template for Azure Resource Manager, follow the below steps.

1.	Go to **Azure portal**. (https://portal.azure.com)

2.	Navigate to **Create a resource (+)**, search for **Template deployment**.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd2.png)

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd3.PNG)

3.	Click **Create** button and click **Build your own Template** in the editor as shown in the following figure.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd4.PNG)

4.	The **Edit template** page is displayed as shown in the following figure.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd5.PNG)

5.	**Replace / paste** the template and click **Save** button.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd6.PNG)

6.	The **Custom deployment** page is displayed as shown in the following figure.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd7.PNG)
![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd8.PNG)
![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd9.PNG)

To deploy the template, we need to create the **resource group** for that.

To create the new **resource group** Click **Create New** and provide the **Name** of the resource group.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd10.PNG)

The following are the **parameters** to be passed while deploying the template.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd11.PNG)
![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd12.PNG)
![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd13.PNG)

**Note:** 
  1.	**tenantId** : Provide your Azure tenantId
  2.  **servicePrincipalClientId** : Provide Service principal Client ID
  3.  **servicePrincipalClientSecret** : Provide Service principal Client Secret
  4.	**ClientID** : Provide the AD Client ID
  5.	**Domain** : Provide AD Domain name
  6.  **AD tenantID** : Provide AD tenantID
  7.  **Azure AD B2C ClientID** : Provide AzureAD B2C client ID
  8.  **AzureAD B2C domain** : Provide AzureAD B2C domain
  9.  **signUpSignInPolicyId** : Provide SignUpSignInPolicyId of AzureAD B2CWeb.
  10.	**sessionId** : Generate an online Guid id (for each deployment it should be unique)
      to generate GUID follow the below url
 https://www.guidgenerator.com/online-guid-generator.aspx


 7.Once all the parameters are entered, check in the **terms and conditions** check box and click **Purchase**.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd14.PNG)

8.	 After the successful deployment of the ARM template, the following **resources** are created in a **Resource Group**.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd15.PNG)

The following are the list of resources were deployed after successful deployment of the ARM template.

  1.	IoT hub
  2.	Web API & Web Admin 
  3.	Storage Account blob
  4.	Cosmos DB
  5.	Notification hub
  6.	Account Automation
  7.	Kubernetes cluster
  8.	Omsworkspace


![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd16.PNG)
![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd17.PNG)
![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd19.PNG)

To check the Deployemnts follow the below steps.
Go to **Resource Group** -> Click **Deployments**.

![alt text](https://github.com/ChaitanyaGeddam/edison/raw/master/images/EdisonRmd18.PNG)


 

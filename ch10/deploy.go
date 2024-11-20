package main

import (
	"context"
	"encoding/json"
	"fmt"
	"log"
	"os"

	"github.com/Azure/azure-sdk-for-go/sdk/azcore/to"
	"github.com/Azure/azure-sdk-for-go/sdk/azidentity"
	"github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/resources/armresources"
)

const (
	resourceGroupName = "<resource-group-name>"
	deploymentName    = "<deployment-name>"
	templateFile      = "aci.json"
)

var (
	ctx = context.Background()
)

func readJSON(path string) (map[string]interface{}, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		log.Fatalf("failed to read file: %v", err)
	}
	contents := make(map[string]interface{})
	_ = json.Unmarshal(data, &contents)
	return contents, nil
}

func main() {
	subscriptionId := os.Getenv("AZURE_SUBSCRIPTION_ID")

	cred, err := azidentity.NewDefaultAzureCredential(nil)
	if err != nil {
		log.Fatalf("failed to obtain a credential: %v", err)
	}

	template, err := readJSON(templateFile)
	if err != nil {
		return
	}

	deploymentsClient, err := armresources.NewDeploymentsClient(subscriptionId, cred, nil)
	if err != nil {
		log.Fatalf("failed to create client: %v", err)
	}
	deploy, err := deploymentsClient.BeginCreateOrUpdate(
		ctx,
		resourceGroupName,
		deploymentName,
		armresources.Deployment{
			Properties: &armresources.DeploymentProperties{
				Template:   template,
				Parameters: "{\"parCustomerId\": {\"value\": \"18363875\"}}",
				Mode:       to.Ptr(armresources.DeploymentModeIncremental),
			},
		},
		nil,
	)
	if err != nil {
		log.Fatalf("failed to deploy template: %v", err)
	}

	fmt.Println(deploy)
}

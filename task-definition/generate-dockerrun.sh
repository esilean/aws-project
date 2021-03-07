#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
DIROUT="$( cd ../"$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

TASK_DEFINITION_INPUT_FILE="awsproject.dockerrun.template"
TASK_DEFINITION_INPUT_PATH="${DIR}/template/${TASK_DEFINITION_INPUT_FILE}"

TASK_DEFINITION_OUTPUT_FILE="Dockerrun.aws.json"
TASK_DEFINITION_OUTPUT_PATH="${DIROUT}/${TASK_DEFINITION_OUTPUT_FILE}"

echo "Configuring region to: ${AWS_REGION}"
echo "Configuring account id to: ${ACCOUNT_ID}"

sed -e 's/\$\$AWS_REGION\$\$/'"${AWS_REGION}"'/g' -e 's/\$\$ACCOUNT_ID\$\$/'"${ACCOUNT_ID}"'/g' -e 's/\$\$VERSION\$\$/'"${CODEBUILD_RESOLVED_SOURCE_VERSION}"'/g' ${TASK_DEFINITION_INPUT_PATH} > ${TASK_DEFINITION_OUTPUT_PATH}

echo "Task Definition written to: ${TASK_DEFINITION_OUTPUT_PATH}"

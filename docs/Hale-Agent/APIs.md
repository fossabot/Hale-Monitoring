#  Generic Agent APIs #


##  Response Schema ##

The response schema is very similar to the schema used for check responses. The only difference as off the current push is that it lacks the performance vector, of obvious reasons.


      {
        "$schema": "",
        "id": "",
        "type": "object",
        "properties": {
          "HostName": {
            "id": "/HostName",
            "type": "string"
          },
          "HostIPs": {
            "id": "/HostIPs",
            "type": "array",
            "items": {
              "id": "/HostIPs/0",
              "type": "string"
            }
          },
          "Origin": {
            "id": "/Origin",
            "type": "null"
          },
          "Timestamp": {
            "id": "/Timestamp",
            "type": "string"
          },
          "Status": {
            "id": "/Status",
            "type": "integer"
          },
          "Text": {
            "id": "/Text",
            "type": "array",
            "items": [
              {
                "id": "/Text/0",
                "type": "string"
              },
              {
                "id": "/Text/1",
                "type": "string"
              }
            ]
          },
          "Exceptions": {
            "id": "/Exceptions",
            "type": "array",
            "items": []
          }
        },
        "required": [
          "HostName",
          "HostIPs",
          "Origin",
          "Timestamp",
          "Status",
          "Text",
          "Exceptions"
        ]
      }

## Example ##

    {
       "HostName":"gaudi",
       "HostIPs":[
          "10.0.0.99"
       ],
       "Origin":null,
       "Timestamp":"2015-07-13 22:53:45",
       "Status":0,
       "Text":[
          "Disk Space",
          "Memory Usage"
       ],
       "Exceptions":[
    
       ]
    }

## APIs and Explanations ##
| URL   | Actions    | Description |
|-------|------------|-------------|
|/agent |            |             |
|/checks|            |             |


# The Check API #
| URL   | Actions    | Description |
|-------|------------|-------------|
|/check |            |             |

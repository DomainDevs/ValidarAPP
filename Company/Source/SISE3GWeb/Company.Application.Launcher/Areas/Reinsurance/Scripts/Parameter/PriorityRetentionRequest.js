class PriorityRetentionRequest{

    static GetLineBusiness() {
        return $.ajax({
            async : false,
            type: 'GET',
            url: rootPath + 'Reinsurance/Parameter/GetLineBusiness',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPriorityRetentions() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Reinsurance/Parameter/GetPriorityRetentions',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SavePriorityRetentions(lstPriorityRetentionAdded, lstPriorityRetentionModified, lstPriorityRetentionDelete) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Parameter/SavePriorityRetentions',
            data: JSON.stringify({ lstPriorityRetentionAdded: lstPriorityRetentionAdded, lstPriorityRetentionModified: lstPriorityRetentionModified, lstPriorityRetentionDelete: lstPriorityRetentionDelete }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CanPriorityRetentionUpdated(priorityRetentionId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Parameter/CanPriorityRetentionUpdated',
            data: JSON.stringify({ priorityRetentionId: priorityRetentionId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static getPrefixes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Reinsurance/Process/GetPrefix',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    
}
namespace SerilogEcsLogging.Logging;

public class EcsDocument : Elastic.CommonSchema.EcsDocument
{
    public const string DataFieldName = "Data";

    public object? Data { get; set; }

    public EcsDocument()
    {
    }

    public EcsDocument(Elastic.CommonSchema.EcsDocument o, object? data)
    {
        Metadata = o.Metadata;
        Agent = o.Agent;
        As = o.As;
        Client = o.Client;
        Cloud = o.Cloud;
        CodeSignature = o.CodeSignature;
        Container = o.Container;
        DataStream = o.DataStream;
        Destination = o.Destination;
        Device = o.Device;
        Dll = o.Dll;
        Dns = o.Dns;
        Ecs = o.Ecs;
        Elf = o.Elf;
        Email = o.Email;
        Error = o.Error;
        Event = o.Event;
        Faas = o.Faas;
        File = o.File;
        Geo = o.Geo;
        Group = o.Group;
        Hash = o.Hash;
        Host = o.Host;
        Http = o.Http;
        Interface = o.Interface;
        Log = o.Log;
        Network = o.Network;
        Observer = o.Observer;
        Orchestrator = o.Orchestrator;
        Organization = o.Organization;
        Os = o.Os;
        Package = o.Package;
        Pe = o.Pe;
        Process = o.Process;
        Registry = o.Registry;
        Related = o.Related;
        Risk = o.Risk;
        Rule = o.Rule;
        Server = o.Server;
        Service = o.Service;
        Source = o.Source;
        Threat = o.Threat;
        Tls = o.Tls;
        Url = o.Url;
        User = o.User;
        UserAgent = o.UserAgent;
        Vlan = o.Vlan;
        Vulnerability = o.Vulnerability;
        Timestamp = o.Timestamp;
        Message = o.Message;
        Tags = o.Tags;
        SpanId = o.SpanId;
        TraceId = o.TraceId;
        TransactionId = o.TransactionId;
        Labels = o.Labels;
        
        Data = data;
    }

    protected override bool TryRead(string propertyName, out Type? type)
    {
        switch (propertyName)
        {
            case DataFieldName:
                type = typeof(object);
                return true;
        }
        
        return base.TryRead(propertyName, out type);
    }

    protected override bool ReceiveProperty(string propertyName, object value)
    {
        switch (propertyName)
        {
            case DataFieldName:
                Data = value;
                return true;
        }
        
        return base.ReceiveProperty(propertyName, value);
    }

    protected override void WriteAdditionalProperties(Action<string, object> write)
    {
        if (Data != null)
        {
            write(DataFieldName, Data);
        }
        
        base.WriteAdditionalProperties(write);
    }
}
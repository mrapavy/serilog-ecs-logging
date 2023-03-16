namespace SerilogEcsLogging.Logging;

public class Base : Elastic.CommonSchema.Base
{
    public const string DataFieldName = "Data";

    public object? Data { get; set; }

    public Base()
    {
    }

    public Base(Elastic.CommonSchema.Base o, object data)
    {
        Metadata = o.Metadata;
        Agent = o.Agent;
        As = o.As;
        Client = o.Client;
        Cloud = o.Cloud;
        CodeSignature = o.CodeSignature;
        Container = o.Container;
        Destination = o.Destination;
        Dll = o.Dll;
        Dns = o.Dns;
        Ecs = o.Ecs;
        Error = o.Error;
        Event = o.Event;
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
        Organization = o.Organization;
        Os = o.Os;
        Package = o.Package;
        Pe = o.Pe;
        Process = o.Process;
        Registry = o.Registry;
        Related = o.Related;
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
        Tags = o.Tags;
        Labels = o.Labels;
        Message = o.Message;
        Trace = o.Trace;
        Transaction = o.Transaction;
        Data = data;
    }

    protected override bool TryRead(string propertyName, out Type type)
    {
        if (propertyName == DataFieldName)
        {
            type = typeof(object);
            return true;
        }

        return base.TryRead(propertyName, out type);
    }

    protected override bool ReceiveProperty(string propertyName, object value)
    {
        if (propertyName == DataFieldName)
        {
            Data = value;
            return true;
        }
        
        return base.ReceiveProperty(propertyName, value);
    }

    protected override void WriteAdditionalProperties(Action<string, object?> write)
    {
        write(DataFieldName, Data);
    }
}
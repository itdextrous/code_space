namespace SAASCLOUDAPP.DataAccessLayer.Interfaces
{
    public interface IWidgetType
    {
        /// <summary>
        /// The general type of the widget.
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// The specific type of the widget. Defines how the widget is displayed and behaves.
        /// </summary>
        string WidgetFor { get; set; }
    }
}
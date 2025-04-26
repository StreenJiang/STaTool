using Dapper.Contrib.Extensions;
using StaTool.attribute;
using StaTool.constants;

namespace STaTool.db.models {
    [Table("tightening_data")]
    public class TighteningData: AEntityBase {
        [FieldName("IP地址", 1)]
        public string? tool_ip { get; set; }                                                // 工具IP
        [FieldName("端口号", 2)]
        public int? tool_port { get; set; }                                                 // 工具端口
        public int? cell_id { get; set; }                                                   //
        public int? channel_id { get; set; }                                                //
        [FieldName("控制器名称", 3)]
        public string? torque_controller_name { get; set; }                                 //
        [FieldName("vin号", 4)]
        public string? vin_number { get; set; }                                             // 
        public int? job_id { get; set; }                                                    // 
        [FieldName("程序号", 5)]
        public int? parameter_set_id { get; set; }                                      // 程序号
        public int? batch_size { get; set; }                                                // 批次总数量
        public int? batch_counter { get; set; }                                             // 批次计数
        [FieldName("拧紧结果", 6, typeof(TighteningStatus))]
        public int? tightening_status { get; set; }                                         // 最终状态
        [FieldName("扭矩结果", 7, typeof(ValueStatus))]
        public int? torque_status { get; set; }                                             // 最终扭力状态
        [FieldName("角度结果", 8, typeof(ValueStatus))]
        public int? angle_status { get; set; }                                              // 最终角度状态
        [FieldName("扭矩下限", 9)]
        public double? torque_min_limit { get; set; }                                        // 最终扭力下限
        [FieldName("扭矩上限", 10)]
        public double? torque_max_limit { get; set; }                                        // 最终扭力上限
        [FieldName("扭矩目标值", 11)]
        public double? torque_final_target { get; set; }                                     // 最终扭力目标值
        [FieldName("扭矩值", 12)]
        public double? torque { get; set; }                                                  // 最终扭力
        [FieldName("角度值", 13)]
        public int? angle_min { get; set; }                                                 // 最终角度下限
        [FieldName("角度值", 14)]
        public int? angle_max { get; set; }                                                 // 最终角度上限
        [FieldName("角度值", 15)]
        public int? angle_final_target { get; set; }                                        // 最终角度目标值
        [FieldName("角度值", 16)]
        public int? angle { get; set; }                                                     // 最终角度
        [FieldName("时间戳", 17)]
        public string? timestamp { get; set; }                                              // 拧紧时间戳记
        public string? date_or_time_of_last_change_in_parameter_set_settings { get; set; }  // 
        public int? batch_status { get; set; }                                              // 批次状态
        [FieldName("拧紧ID", 18)]
        public int? tightening_id { get; set; }                                             // 
    }
}

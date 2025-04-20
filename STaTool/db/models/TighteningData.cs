using Dapper.Contrib.Extensions;

namespace STaTool.db.models {
    [Table("tightening_data")]
    public class TighteningData: AEntityBase {
        public string? tool_ip { get; set; }                                                // 工具IP
        public int? tool_port { get; set; }                                                 // 工具端口
        public int? cell_id { get; set; }                                                   //
        public int? channel_id { get; set; }                                                //
        public string? torque_controller_name { get; set; }                                 //
        public string? vin_number { get; set; }                                             // 
        public int? job_id { get; set; }                                                    // 
        public int? parameter_set_number { get; set; }                                      // 程序号
        public int? batch_size { get; set; }                                                // 批次总数量
        public int? batch_counter { get; set; }                                             // 批次计数
        public int? tightening_status { get; set; }                                         // 最终状态
        public int? torque_status { get; set; }                                             // 最终扭力状态
        public int? angle_status { get; set; }                                              // 最终角度状态
        public double? torque_min_limit { get; set; }                                        // 最终扭力下限
        public double? torque_max_limit { get; set; }                                        // 最终扭力上限
        public double? torque_final_target { get; set; }                                     // 最终扭力目标值
        public double? torque { get; set; }                                                  // 最终扭力
        public int? angle_min { get; set; }                                                 // 最终角度下限
        public int? angle_max { get; set; }                                                 // 最终角度上限
        public int? angle_final_target { get; set; }                                        // 最终角度目标值
        public int? angle { get; set; }                                                     // 最终角度
        public string? timestamp { get; set; }                                              // 拧紧时间戳记
        public string? date_or_time_of_last_change_in_parameter_set_settings { get; set; }  // 
        public int? batch_status { get; set; }                                              // 批次状态
        public int? tightening_id { get; set; }                                             // 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThiVeMyThuat
{
    class CPhongThi
    {
        public List<vemt> Lst_baithi_theophong = new List<vemt>();
        public int sobaithi_con_theophong { get; set; }
    }

    class CTui
    {
        public List<vemt> Lst_baithi_theotui = new List<vemt>();
        public int sobaithi_con_theotui { get; set; }
    }
    class Utils
    {
        //Khởi tạo list phòng thi
        public static List<CPhongThi> lst_phongthi = new List<CPhongThi>();
        //Khởi tạo list túi
        public static List<CTui> lst_tui = new List<CTui>();
        //khởi tạo biến đếm số túi bài thi
        public static int sotuibaithi { get; set; }

        public static void LoadFullScreen(Form f)
        {
            f.WindowState = FormWindowState.Maximized;
        }

        public static void TronTuiBaiThi(int sobaithi_theotui)
        {
            #region chặt nhỏ dữ liệu chia ra thành các phòng thi khác nhau

            //reset lst_tui và lst_phongthi
            lst_tui = new List<CTui>();
            lst_phongthi = new List<CPhongThi>();

            //lấy về dữ liệu
            dbVeMTDataContext db = new dbVeMTDataContext();
            //var db_sort = db.vemts.OrderBy(o => o.sobaodanh).OrderBy(o => o.phongthi);            

            int sophongthi = 30; //sau này cần lấy động số phòng thi.

            //loop so phong thi để chuẩn bị dữ liệu theo từng phòng thi
            for (int i = 0; i < sophongthi; i++)
            {
                //tạo ra đối tượng phòng thi 
                CPhongThi phongthi = new CPhongThi();

                //lấy về toàn bộ số thí sinh theo phòng thi thứ i+1.
                var lst_thisinh = db.vemts.Where(o => o.phongthi == i + 1);
                if (lst_thisinh.Count() > 0)
                {
                    //nếu số thí sinh trong 1 phòng thi  > 0 thì chuyển toàn bộ dữ liệu của thí sinh vào trong đối tượng phòng thi
                    foreach (var ts in lst_thisinh)
                    {
                        phongthi.Lst_baithi_theophong.Add(ts);
                    }
                }

                //add phòng thi vừa có vào lst phòng thi
                lst_phongthi.Add(phongthi);
            }
            //đến đây đã có toàn bộ dữ liệu về từng phòng thi.
            #endregion

            #region thực hiện trộn túi bài thi

            //tính số bài thi để chia đoạn phòng thi
            int chiadoan = sobaithi_theotui / 3 + 1;
            //tính có bao nhiêu túi bài thi cần trộn
            sotuibaithi = ((int)db.vemts.Count() / sobaithi_theotui) + 1;

            for (int i = 0; i < sotuibaithi; i++)
            {
                //khởi tạo đối tượng để random phòng thi cho mỗi lần lấy bài thi vào túi
                Random r = new Random();

                //khởi tạo 1 túi bài thi
                CTui tui = new CTui();
                tui.sobaithi_con_theotui = sobaithi_theotui;
                while (tui.sobaithi_con_theotui > 0 && lst_phongthi.Count() > 0)
                {

                    int k = r.Next(0, lst_phongthi.Count - 1);
                    //MessageBox.Show(ar[index].ToString());

                    ////lấy random phòng thi tại đây. Ở đây chưa random nên lấy tuần tự
                    //for (int k = 0; k < lst_phongthi.Count(); k++)
                    //{

                    CPhongThi pt = lst_phongthi[k];

                    //kiểm tra xem phòng thi này còn bài thi k
                    pt.sobaithi_con_theophong = pt.Lst_baithi_theophong.Count();
                    if (pt.sobaithi_con_theophong > 0)
                    {
                        //nếu còn bài thi trong phòng thì kiểm tra xem nó có > 13 k?
                        if (pt.sobaithi_con_theophong > chiadoan)
                        {
                            //Đ: kiểm tra số bài thi trong túi cần lấy thêm có > 13 k?
                            if (tui.sobaithi_con_theotui > chiadoan)
                            {
                                //Đ: lấy tuần tự 13 bài từ phòng thi ném vào trong túi hiện tại
                                Move_record_from_pt_to_tui(tui, pt, chiadoan);
                            }
                            else
                            {
                                //S: lấy toàn bộ số bài thi còn trong phòng ném vào trong túi.
                                Move_record_from_pt_to_tui(tui, pt, tui.sobaithi_con_theotui);
                            }

                            //update số bài thi còn
                            Update_sobaithi_con(tui, pt, sobaithi_theotui);
                        }
                        else
                        {
                            //S: kiểm tra số bài thi cần thêm vào túi có lớn hơn số bài thi còn trong phòng k
                            if (tui.sobaithi_con_theotui >= pt.sobaithi_con_theophong)
                            {
                                //Đ: lấy hết số bài thi còn trong phòng
                                Move_record_from_pt_to_tui(tui, pt, pt.sobaithi_con_theophong);

                            }
                            else
                            {
                                /********************************************************/
                                #region summary: đoạn này để chống phân mảnh bài thi.
                                //----------------------------------------------------------------

                                //S: kiểm tra số phòng thi còn trong lst_phongthi >2 hay không
                                if (lst_phongthi.Count() > 2)
                                {
                                    //Đ: kiểm tra số lượng bài thi còn dư lại nếu lấy = số bài thi cần thêm vào túi
                                    int sobaithi_con_saukhi_laydu_theotui = pt.sobaithi_con_theophong - tui.sobaithi_con_theotui;
                                    if (sobaithi_con_saukhi_laydu_theotui > 3)
                                    {
                                        //Đ: Lấy = số bài thi cần thêm vào túi.
                                        Move_record_from_pt_to_tui(tui, pt, tui.sobaithi_con_theotui);
                                    }
                                }
                                else
                                {
                                    //------------------------------------------------------------------------
                                #endregion
                                    /********************************************************/
                                    //S: Lấy = số bài thi cần thêm vào túi.
                                    Move_record_from_pt_to_tui(tui, pt, tui.sobaithi_con_theotui);
                                }


                            }

                            //update số bài thi còn
                            Update_sobaithi_con(tui, pt, sobaithi_theotui);
                        }
                    }
                    else
                    {
                        //nếu phòng thi này k còn bài thi nào -> loại ra khỏi list_phongthi
                        lst_phongthi.Remove(pt);
                    }

                    //nếu đã lấy đủ số bài thi cho túi thì break loop.
                    if (tui.sobaithi_con_theotui == 0)
                    {
                        break;
                    }
                }

                //}

                //sau khi chèn đủ số bài thi vào túi -> add vào list_tui
                lst_tui.Add(tui);
            }
            #endregion

            #region đánh số túi cho từng túi bài thi
            Danh_sotui();
            #endregion

        }

        private static void Update_sobaithi_con(CTui tui, CPhongThi pt, int sobaithi_theotui)
        {
            //update lại số bài thi trong túi
            tui.sobaithi_con_theotui = sobaithi_theotui - tui.Lst_baithi_theotui.Count();
            //update lại số bài thi trong phòng
            pt.sobaithi_con_theophong = pt.Lst_baithi_theophong.Count();
        }

        private static void Move_record_from_pt_to_tui(CTui tui, CPhongThi pt, int num_loop)
        {
            for (int j = 0; j < num_loop; j++)
            {
                tui.Lst_baithi_theotui.Add(pt.Lst_baithi_theophong[0]);
                pt.Lst_baithi_theophong.RemoveAt(0);
            }
        }

        private static void Danh_sotui ()
        {
            for (int i = 0; i < lst_tui.Count(); i++)
            {
                for (int j = 0; j < lst_tui[i].Lst_baithi_theotui.Count(); j++)
                {
                    lst_tui[i].Lst_baithi_theotui[j].tui = i + 1;
                    lst_tui[i].Lst_baithi_theotui[j].stttui = j + 1;
                }
            }
        }

    }
}

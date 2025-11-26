const api = {
  lops: '/api/lops',
  sinhviens: '/api/sinhviens'
};
api.dangky = '/api/dangky';
api.query = '/api/query';

// --- STATUS / DB CHECK ---
async function fetchStatus(){
  try{
    const res = await fetch('/api/check');
    const banner = document.getElementById('statusBanner');
    if(!res.ok){
      const text = await res.text();
      if(banner) banner.textContent = 'Kết nối DB lỗi: ' + text;
      return;
    }
    const data = await res.json();
    if(banner){
      let details = '';
      if(data.details){
        for(const k in data.details){
          details += `${k}: ${data.details[k]}  `;
        }
      }
      banner.textContent = (data.connectionStringSet ? 'Kết nối DB: OK. ' : 'Chuỗi kết nối chưa đặt. ') + details;
    }
  }catch(e){
    const banner = document.getElementById('statusBanner');
    if(banner) banner.textContent = 'Lỗi khi kiểm tra DB: ' + e.message;
  }
}


// --- LOP ---
const lopTableBody = document.querySelector('#lopTable tbody');
const refreshLopsBtn = document.getElementById('refreshLops');
const showAddLopBtn = document.getElementById('showAddLop');
const lopForm = document.getElementById('lopForm');
const cancelLop = document.getElementById('cancelLop');
const saveLop = document.getElementById('saveLop');

async function fetchLops(){
  const res = await fetch(api.lops);
  if(!res.ok){ alert('Lấy dữ liệu Lớp thất bại: ' + res.status); return; }
  const data = await res.json();
  lopTableBody.innerHTML = '';
  data.forEach(l => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td>${l.MaLop}</td>
      <td>${l.TenLop}</td>
      <td>${l.Khoa}</td>
      <td>${l.Site}</td>
      <td>
        <button data-act="edit" data-malop="${l.MaLop}" data-site="${l.Site}">Sửa</button>
        <button data-act="del" data-malop="${l.MaLop}" data-site="${l.Site}" class="secondary">Xóa</button>
      </td>
    `;
    lopTableBody.appendChild(tr);
  });
}

refreshLopsBtn.addEventListener('click', fetchLops);
showAddLopBtn.addEventListener('click', ()=> lopForm.classList.toggle('hidden'));
cancelLop.addEventListener('click', ()=> lopForm.classList.add('hidden'));

// Xác định hàm xử lý POST mặc định
async function handleSaveLopPost(){
  const lop = {
    MaLop: document.getElementById('lopMaLop').value,
    TenLop: document.getElementById('lopTenLop').value,
    Khoa: document.getElementById('lopKhoa').value,
    Site: document.getElementById('lopSite').value
  };
  const res = await fetch(api.lops, {method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(lop)});
  if(!res.ok){ alert('Lưu Lớp thất bại: ' + (await res.text())); return; }
  lopForm.classList.add('hidden');
  fetchLops();
}

saveLop.addEventListener('click', handleSaveLopPost);

// Hàm đặt lại sự kiện lưu về mặc định
function resetSaveLopHandler(){
  saveLop.onclick = null;
  saveLop.addEventListener('click', handleSaveLopPost);
}

lopTableBody.addEventListener('click', async (e)=>{
  const btn = e.target.closest('button');
  if(!btn) return;
  const act = btn.dataset.act;
  const ma = btn.dataset.malop;
  const site = btn.dataset.site;
  if(act==='del'){
    if(confirm('Xác nhận xóa?')){
      await fetch(`${api.lops}/${encodeURIComponent(ma)}/${encodeURIComponent(site)}`,{method:'DELETE'});
      fetchLops();
    }
  } else if(act==='edit'){
    // load into form
    try {
      const res = await fetch(`${api.lops}/${encodeURIComponent(ma)}/${encodeURIComponent(site)}`);
      if(!res.ok){ alert('Lấy 1 lớp thất bại: ' + res.status); return; }
      const data = await res.json();
      document.getElementById('lopMaLop').value = data.MaLop;
      document.getElementById('lopTenLop').value = data.TenLop;
      document.getElementById('lopKhoa').value = data.Khoa;
      document.getElementById('lopSite').value = data.Site;
      lopForm.classList.remove('hidden');
      // Xóa sự kiện POST mặc định trước khi thêm sự kiện PUT
      saveLop.removeEventListener('click', handleSaveLopPost);
      // change save to PUT temporarily
      saveLop.onclick = async ()=>{
        const updated = {
          MaLop: document.getElementById('lopMaLop').value,
          TenLop: document.getElementById('lopTenLop').value,
          Khoa: document.getElementById('lopKhoa').value,
          Site: document.getElementById('lopSite').value
        };
        await fetch(`${api.lops}/${encodeURIComponent(ma)}/${encodeURIComponent(site)}`,{
          method:'PUT',headers:{'Content-Type':'application/json'},body:JSON.stringify(updated)
        });
        // Đặt lại sự kiện lưu về mặc định
        resetSaveLopHandler();
        lopForm.classList.add('hidden');
        fetchLops();
      };
    } catch (error) {
      alert('Lỗi khi tải dữ liệu lớp: ' + error.message);
      // Đặt lại sự kiện lưu nếu có lỗi
      resetSaveLopHandler();
    }
  }
});

// --- SINH VIEN ---
const svTableBody = document.querySelector('#svTable tbody');
const refreshSVBtn = document.getElementById('refreshSV');
const showAddSVBtn = document.getElementById('showAddSV');
const svForm = document.getElementById('svForm');
const cancelSV = document.getElementById('cancelSV');
const saveSV = document.getElementById('saveSV');

async function fetchSV(){
  const res = await fetch(api.sinhviens);
  if(!res.ok){ alert('Lấy danh sách Sinh Viên thất bại: ' + res.status); return; }
  const data = await res.json();
  svTableBody.innerHTML = '';
  data.forEach(s => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td>${s.MaSV}</td>
      <td>${s.HoTen}</td>
      <td>${s.Phai}</td>
      <td>${new Date(s.NgaySinh).toISOString().slice(0,10)}</td>
      <td>${s.MaLop}</td>
      <td>${s.HocBong}</td>
      <td>${s.Site}</td>
      <td>
        <button data-act="edit" data-ma="${s.MaSV}" data-site="${s.Site}">Sửa</button>
        <button data-act="del" data-ma="${s.MaSV}" data-site="${s.Site}" class="secondary">Xóa</button>
      </td>
    `;
    svTableBody.appendChild(tr);
  });
}

refreshSVBtn.addEventListener('click', fetchSV);
showAddSVBtn.addEventListener('click', ()=> svForm.classList.toggle('hidden'));
cancelSV.addEventListener('click', ()=> svForm.classList.add('hidden'));

// Xác định hàm xử lý POST mặc định cho Sinh Viên
async function handleSaveSVPost(){
  const sv = {
    MaSV: document.getElementById('svMaSV').value,
    HoTen: document.getElementById('svHoTen').value,
    Phai: document.getElementById('svPhai').value,
    NgaySinh: new Date(document.getElementById('svNgaySinh').value).toISOString().slice(0,10),
    MaLop: document.getElementById('svMaLop').value,
    HocBong: parseFloat(document.getElementById('svHocBong').value||0),
    Khoa: document.getElementById('svKhoa').value,
    Site: document.getElementById('svSite').value
  };
  const res = await fetch(api.sinhviens, {method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(sv)});
  if(!res.ok){ alert('Lưu Sinh Viên thất bại: ' + res.status + ' - ' + (await res.text())); return; }
  svForm.classList.add('hidden');
  fetchSV();
}

saveSV.addEventListener('click', handleSaveSVPost);

// Hàm đặt lại sự kiện lưu về mặc định cho Sinh Viên
function resetSaveSVHandler(){
  saveSV.onclick = null;
  saveSV.addEventListener('click', handleSaveSVPost);
}


svTableBody.addEventListener('click', async (e)=>{
  const btn = e.target.closest('button');
  if(!btn) return;
  const act = btn.dataset.act;
  const ma = btn.dataset.ma;
  const site = btn.dataset.site;
  if(act==='del'){
    if(confirm('Xác nhận xóa?')){
      await fetch(`${api.sinhviens}/${encodeURIComponent(ma)}/${encodeURIComponent(site)}`,{method:'DELETE'});
      fetchSV();
    }
  } else if(act==='edit'){
    try {
      const res = await fetch(`${api.sinhviens}/${encodeURIComponent(ma)}/${encodeURIComponent(site)}`);
      if(!res.ok){ alert('Lấy sinh viên thất bại: ' + res.status); return; }
      const data = await res.json();
      document.getElementById('svMaSV').value = data.MaSV;
      document.getElementById('svHoTen').value = data.HoTen;
      document.getElementById('svPhai').value = data.Phai;
      document.getElementById('svNgaySinh').value = new Date(data.NgaySinh).toISOString().slice(0,10);
      document.getElementById('svMaLop').value = data.MaLop;
      document.getElementById('svHocBong').value = data.HocBong;
      document.getElementById('svKhoa').value = data.Khoa;
      document.getElementById('svSite').value = data.Site;
      svForm.classList.remove('hidden');
      // Xóa sự kiện POST mặc định trước khi thêm sự kiện PUT
      saveSV.removeEventListener('click', handleSaveSVPost);
      saveSV.onclick = async ()=>{
        const updated = {
          MaSV: document.getElementById('svMaSV').value,
          HoTen: document.getElementById('svHoTen').value,
          Phai: document.getElementById('svPhai').value,
          NgaySinh: document.getElementById('svNgaySinh').value,
          MaLop: document.getElementById('svMaLop').value,
          HocBong: parseFloat(document.getElementById('svHocBong').value||0),
          Khoa: document.getElementById('svKhoa').value,
          Site: document.getElementById('svSite').value
        };
        await fetch(`${api.sinhviens}/${encodeURIComponent(ma)}/${encodeURIComponent(site)}`,{
          method:'PUT',headers:{'Content-Type':'application/json'},body:JSON.stringify(updated)
        });
        resetSaveSVHandler();
        svForm.classList.add('hidden');
        fetchSV();
      };
    } catch (error) {
      alert('Lỗi khi tải dữ liệu sinh viên: ' + error.message);
      resetSaveSVHandler();
    }
  }
});

// initial load
fetchLops();
fetchSV();
fetchStatus();

// --- DANG KY ---
const dkTableBody = document.querySelector('#dkTable tbody');
const refreshDKBtn = document.getElementById('refreshDK');
const showAddDKBtn = document.getElementById('showAddDK');
const dkForm = document.getElementById('dkForm');
const cancelDK = document.getElementById('cancelDK');
const saveDK = document.getElementById('saveDK');

async function fetchDK(){
  try{
    const res = await fetch(api.dangky);
    if(!res.ok) return dkTableBody.innerHTML='';
    const data = await res.json();
    dkTableBody.innerHTML = '';
    data.forEach(d => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${d.MaSV}</td>
        <td>${d.MaMon}</td>
        <td>${d.Diem1 ?? ''}</td>
        <td>${d.Diem2 ?? ''}</td>
        <td>${d.Diem3 ?? ''}</td>
        <td>${d.Site}</td>
        <td>
          <button data-act="edit" data-ma="${d.MaSV}" data-mamon="${d.MaMon}" data-site="${d.Site}">Sửa</button>
          <button data-act="del" data-ma="${d.MaSV}" data-mamon="${d.MaMon}" data-site="${d.Site}" class="secondary">Xóa</button>
        </td>
      `;
      dkTableBody.appendChild(tr);
    });
  }catch(e){ console.error(e) }
}

refreshDKBtn?.addEventListener('click', fetchDK);
showAddDKBtn?.addEventListener('click', ()=> dkForm.classList.toggle('hidden'));
cancelDK?.addEventListener('click', ()=> dkForm.classList.add('hidden'));

saveDK?.addEventListener('click', async ()=>{
  const dk = {
    MaSV: document.getElementById('dkMaSV').value,
    MaMon: document.getElementById('dkMaMon').value,
    Diem1: parseFloat(document.getElementById('dkDiem1').value||0) || null,
    Diem2: parseFloat(document.getElementById('dkDiem2').value||0) || null,
    Diem3: parseFloat(document.getElementById('dkDiem3').value||0) || null,
    Site: document.getElementById('dkSite').value
  };
  const res = await fetch(api.dangky, {method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(dk)});
  if(!res.ok){ alert('Lưu Đăng Ký thất bại: ' + (await res.text())); return; }
  dkForm.classList.add('hidden');
  fetchDK();
});

dkTableBody?.addEventListener('click', async (e)=>{
  const btn = e.target.closest('button'); if(!btn) return;
  const act = btn.dataset.act; const ma = btn.dataset.ma; const mamon = btn.dataset.mamon; const site = btn.dataset.site;
  if(act==='del'){ if(confirm('Xác nhận xóa?')){ await fetch(`${api.dangky}/${encodeURIComponent(ma)}/${encodeURIComponent(mamon)}/${encodeURIComponent(site)}`,{method:'DELETE'}); fetchDK(); } }
  else if(act==='edit'){
    const res = await fetch(`${api.dangky}/${encodeURIComponent(ma)}/${encodeURIComponent(mamon)}/${encodeURIComponent(site)}`);
    if(!res.ok){ alert('Lấy đăng ký thất bại: ' + res.status); return; }
    const data = await res.json();
    document.getElementById('dkMaSV').value = data.MaSV;
    document.getElementById('dkMaMon').value = data.MaMon;
    document.getElementById('dkDiem1').value = data.Diem1 ?? '';
    document.getElementById('dkDiem2').value = data.Diem2 ?? '';
    document.getElementById('dkDiem3').value = data.Diem3 ?? '';
    document.getElementById('dkSite').value = data.Site;
    dkForm.classList.remove('hidden');
    saveDK.onclick = async ()=>{
      const body = { Diem1: parseFloat(document.getElementById('dkDiem1').value||0) || null, Diem2: parseFloat(document.getElementById('dkDiem2').value||0) || null, Diem3: parseFloat(document.getElementById('dkDiem3').value||0) || null };
      await fetch(`${api.dangky}/${encodeURIComponent(ma)}/${encodeURIComponent(mamon)}/${encodeURIComponent(site)}`,{method:'PUT',headers:{'Content-Type':'application/json'},body:JSON.stringify(body)});
      saveDK.onclick = null; dkForm.classList.add('hidden'); fetchDK();
    };
  }
});

// --- QUERY ---
const runQ1 = document.getElementById('runQ1');
const runQ2 = document.getElementById('runQ2');
const runQ3 = document.getElementById('runQ3');

runQ1?.addEventListener('click', async ()=>{
  const ma = document.getElementById('q1MaSV').value;
  const res = await fetch(`${api.query}/khoa/${encodeURIComponent(ma)}`);
  const p = document.getElementById('q1Result');
  if(!res.ok){ p.textContent = 'Không tìm thấy'; return; }
  const data = await res.json();
  p.textContent = JSON.stringify(data, null, 2);
});

runQ2?.addEventListener('click', async ()=>{
  const ma = document.getElementById('q2MaSV').value;
  const res = await fetch(`${api.query}/diem/${encodeURIComponent(ma)}`);
  const tbody = document.querySelector('#q2Table tbody');
  tbody.innerHTML='';
  if(!res.ok) return;
  const data = await res.json();
  data.forEach(r=>{
    const tr = document.createElement('tr');
    tr.innerHTML = `<td>${r.MaMon}</td><td>${r.Diem1 ?? ''}</td><td>${r.Diem2 ?? ''}</td><td>${r.Diem3 ?? ''}</td><td>${r.DiemTB ?? ''}</td>`;
    tbody.appendChild(tr);
  });
});

runQ3?.addEventListener('click', async ()=>{
  const res = await fetch(`${api.query}/diemmaxkhoa`);
  const tbody = document.querySelector('#q3Table tbody'); tbody.innerHTML='';
  if(!res.ok) return;
  const data = await res.json();
  data.forEach(r=>{ const tr = document.createElement('tr'); tr.innerHTML = `<td>${r.Khoa}</td><td>${r.DiemTBMax ?? ''}</td>`; tbody.appendChild(tr); });
});

// --- Module nav switching ---
document.querySelectorAll('.nav-btn').forEach(b=>{
  b.addEventListener('click', ()=>{
    const target = b.dataset.target;
    document.querySelectorAll('.module').forEach(m=>m.classList.add('hidden'));
    document.getElementById(target).classList.remove('hidden');
  });
});

// expose dangky list endpoint by adding a simple GET handler consumer expects
// If backend does not support GET /api/dangky, it'll return 404; UI will handle.
fetchDK();

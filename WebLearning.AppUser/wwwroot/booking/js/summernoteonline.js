$('#Title').summernote({
    placeholder: 'Tiêu đề',
    tabsize: 2,
    height: 50,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', /*'video'*/]],
        ['view', [/*'fullscreen'*/, 'codeview', 'help']]
    ]
});
$('#Description').summernote({
    placeholder: 'Nội dung',
    tabsize: 2,
    height: 190,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', /*'video'*/]],
        ['view', [/*'fullscreen'*/, 'codeview', 'help']]]
});
$('#Note').summernote({
    placeholder: 'Ghi chú',
    tabsize: 2,
    height: 120,
    toolbar: [
        ['style', ['style']],
        ['font', ['bold', 'underline', 'clear']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['table', ['table']],
        ['insert', ['link', 'picture', /*'video'*/]],
        ['view', [/*'fullscreen'*/, 'codeview', 'help']]]
});